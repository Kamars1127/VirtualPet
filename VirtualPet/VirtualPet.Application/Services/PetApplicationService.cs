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
        private readonly PetEvolutionService _petEvolutionService;

        public PetApplicationService(IPetRepository petRepository, PetEvolutionService petEvolutionService)
        {
            _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
            _petEvolutionService = petEvolutionService ?? throw new ArgumentNullException(nameof(petEvolutionService));
        }

        /// <summary>
        /// 建立 Pet
        /// </summary>
        /// <param name="name">寵物名稱</param>
        /// <param name="species">寵物種類</param>
        /// <returns></returns>
        public async Task<PetDto> CreatePet(string name, PetSpecies species, CancellationToken cancellationToken = default)
        {
            var pet = new Pet(name, species);

            await _petRepository.AddAsync(pet, cancellationToken);
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

            return PetMapper.ToDto(pet);
        }

        /// <summary>
        /// 取得所有 Pet
        /// </summary>
        public async Task<IReadOnlyList<PetDto>> GetPetsAsync(CancellationToken cancellationToken = default)
        {
            var pets = await _petRepository.GetAllAsync(cancellationToken);

            return pets.Select(PetMapper.ToDto).ToList();
        }

        /// <summary>
        /// 餵食 Pet
        /// </summary>
        /// <returns>是否發生進化</returns>
        public async Task<PetActionResultDto> FeedPetAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            var pet = await GetRequiredPetAsync(petId, cancellationToken);

            pet.Feed();

            var evolved = _petEvolutionService.TryEvolve(pet);

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

            pet.Play();

            var evolved = _petEvolutionService.TryEvolve(pet);

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

            pet.Rest();

            var evolved = _petEvolutionService.TryEvolve(pet);

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

            pet.GainExperience(experience);

            var evolved = _petEvolutionService.TryEvolve(pet);

            await _petRepository.SaveChangesAsync(cancellationToken);

            return new PetActionResultDto { Pet = PetMapper.ToDto(pet), Evolved = evolved };
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
