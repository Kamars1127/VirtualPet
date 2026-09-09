using VirtualPet.Application.DTOs;
using VirtualPet.Application.Mappings;
using VirtualPet.Application.Repositories;
using VirtualPet.Domain.Entities;
using VirtualPet.Domain.Enums;
using VirtualPet.Domain.Services;

namespace VirtualPet.Application.Services
{
    /// <summary>
    /// 處理 Pet 相關 Use Case
    /// </summary>
    public sealed class PetApplicationService
    {
        private readonly IPetRepository _petRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPetHistoryRepository _petHistoryRepository;
        private readonly PetEvolutionService _petEvolutionService;


        public PetApplicationService(IPetRepository petRepository, IUserRepository userRepository, 
                                        IPetHistoryRepository petHistoryRepository, PetEvolutionService petEvolutionService)
        {
            _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _petHistoryRepository = petHistoryRepository ?? throw new ArgumentNullException(nameof(petHistoryRepository));
            _petEvolutionService = petEvolutionService ?? throw new ArgumentNullException(nameof(petEvolutionService));
        }

        /// <summary>
        /// 建立 Pet
        /// </summary>
        /// <param name="name">寵物名稱</param>
        /// <param name="species">寵物種類</param>
        /// <returns></returns>
        public async Task<PetDto> CreatePetAsync(Guid userId, string name, PetSpecies species, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if(user is null)
            {
                throw new KeyNotFoundException($"User with id '{userId}' was not found.");
            }

            var pet = new Pet(userId, name, species);

            await _petRepository.AddAsync(pet, cancellationToken);

            var history = new PetHistory(pet.Id, PetHistoryType.Create, $"Pet '{pet.Name}' was created.");

            await _petHistoryRepository.AddAsync(history, cancellationToken);

            await _petRepository.SaveChangesAsync(cancellationToken);

            return PetMapper.ToDto(pet);
        }

        /// <summary>
        /// 取得指定 Pet
        /// </summary>
        public async Task<PetDto?> GetPetAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            var pet = await _petRepository.GetByIdAsync(petId, cancellationToken);

            if(pet is null) return null;

            var changed = pet.ApplyTimeProgress(DateTime.UtcNow);

            if (changed)
            {
                await _petRepository.SaveChangesAsync(cancellationToken);
            }

            return PetMapper.ToDto(pet);
        }

        /// <summary>
        /// 取得所有 Pet
        /// </summary>
        public async Task<IReadOnlyList<PetDto>> GetPetsAsync(CancellationToken cancellationToken = default)
        {
            var pets = await _petRepository.GetAllAsync(cancellationToken);

            var utcNow = DateTime.UtcNow;
            var changed = false;

            foreach(var pet in pets)
            {
                if (pet.ApplyTimeProgress(utcNow))
                {
                    changed = true;
                }
            }

            if (changed)
            {
                await _petRepository.SaveChangesAsync(cancellationToken);
            }

            return pets.Select(PetMapper.ToDto).ToList();
        }

        /// <summary>
        /// 餵食 Pet
        /// </summary>
        /// <returns>是否發生進化</returns>
        public async Task<PetActionResultDto> FeedPetAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            var pet = await GetRequiredPetAsync(petId, cancellationToken);

            pet.ApplyTimeProgress(DateTime.UtcNow);

            pet.Feed();

            var evolved = _petEvolutionService.TryEvolve(pet);

            var history = new PetHistory(pet.Id, PetHistoryType.Feed, $"Fed pet '{pet.Name}'.");
            await _petHistoryRepository.AddAsync(history, cancellationToken);

            await _petRepository.SaveChangesAsync(cancellationToken);

            return new PetActionResultDto
            {
                Pet = PetMapper.ToDto(pet),
                Evolved = evolved
            };
        }

        /// <summary>
        /// 陪 Pet 玩
        /// </summary>
        /// <returns>是否發生進化</returns>
        public async Task<PetActionResultDto> PlayWithPetAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            var pet = await GetRequiredPetAsync(petId, cancellationToken);
            pet.ApplyTimeProgress(DateTime.UtcNow);
            pet.Play();

            var evolved = _petEvolutionService.TryEvolve(pet);

            var history = new PetHistory(pet.Id, PetHistoryType.Play, $"Played with pet '{pet.Name}'.");
            await _petHistoryRepository.AddAsync(history, cancellationToken);
            await _petRepository.SaveChangesAsync(cancellationToken);

            return new PetActionResultDto
            {
                Pet = PetMapper.ToDto(pet),
                Evolved = evolved
            };
        }

        /// <summary>
        /// 讓 Pet 休息
        /// </summary>
        /// <returns>是否發生進化</returns>
        public async Task<PetActionResultDto> RestPetAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            var pet = await GetRequiredPetAsync(petId, cancellationToken);
            pet.ApplyTimeProgress(DateTime.UtcNow);
            pet.Rest();

            var evolved = _petEvolutionService.TryEvolve(pet);

            var history = new PetHistory(pet.Id, PetHistoryType.Rest, $"Pet '{pet.Name}' rested.");
            await _petHistoryRepository.AddAsync(history, cancellationToken);

            await _petRepository.SaveChangesAsync(cancellationToken);

            return new PetActionResultDto
            {
                Pet = PetMapper.ToDto(pet),
                Evolved = evolved
            };
        }

        /// <summary>
        /// 增加 Pet 經驗值
        /// </summary>
        /// <param name="experience">增加的經驗值</param>
        /// <returns>是否發生進化</returns>
        public async Task<PetActionResultDto> GainExperienceAsync(Guid petId, int experience, CancellationToken cancellationToken = default)
        {
            var pet = await GetRequiredPetAsync(petId, cancellationToken);
            pet.ApplyTimeProgress(DateTime.UtcNow);
            pet.GainExperience(experience);

            var evolved = _petEvolutionService.TryEvolve(pet);

            var history = new PetHistory(pet.Id, PetHistoryType.GainExperience, $"Pet '{pet.Name}' gained {experience} experience.");
            await _petHistoryRepository.AddAsync(history, cancellationToken);

            await _petRepository.SaveChangesAsync(cancellationToken);

            return new PetActionResultDto { Pet = PetMapper.ToDto(pet), Evolved = evolved };
        }

        /// <summary>
        /// 取得某位玩家的所有 Pet
        /// </summary>
        public async Task<IReadOnlyList<PetDto>> GetPetsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var pets = await _petRepository.GetByUserIdAsync(userId, cancellationToken);

            var utcNow = DateTime.UtcNow;
            var changed = false;

            foreach(var pet in pets)
            {
                if (pet.ApplyTimeProgress(utcNow))
                {
                    changed = true;
                }
            }

            if (changed)
            {
                await _petRepository.SaveChangesAsync(cancellationToken);
            }

            return pets.Select(PetMapper.ToDto).ToList();
        }

        public async Task<IReadOnlyList<PetHistoryDto>> GetPetHistoriesAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            await GetRequiredPetAsync(petId, cancellationToken);

            var histories = await _petHistoryRepository.GetByPetIdAsync(petId, cancellationToken);

            return histories.Select(PetHistoryMapper.ToDto).ToList();
        }

        /// <summary>
        /// 取得 Pet，若不存在則拋出例外
        /// </summary>
        private async Task<Pet> GetRequiredPetAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            var pet = await _petRepository.GetByIdAsync(petId, cancellationToken);

            if (pet is null)
                throw new KeyNotFoundException($"Pet with id '{petId}' was not found.");

            return pet;
        }
    }
}
