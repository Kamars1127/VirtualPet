using VirtualPet.Application.DTOs;
using VirtualPet.Application.Mappings;
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
        private readonly PetEvolutionService _petEvolutionService;

        public PetApplicationService(PetEvolutionService petEvolutionService)
        {
            _petEvolutionService = petEvolutionService ?? throw new ArgumentNullException(nameof(petEvolutionService));
        }

        /// <summary>
        /// 建立 Pet
        /// </summary>
        /// <param name="name">寵物名稱</param>
        /// <param name="species">寵物種類</param>
        /// <returns></returns>
        public PetDto CreatePet(string name, PetSpecies species)
        {
            var pet = new Pet(name, species);

            return PetMapper.ToDto(pet);
        }

        /// <summary>
        /// 餵食 Pet
        /// </summary>
        /// <returns>是否發生進化</returns>
        public PetActionResultDto FeedPet(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.Feed();

            var evolved = _petEvolutionService.TryEvolve(pet);

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
        public PetActionResultDto PlayWithPet(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.Play();

            var evolved = _petEvolutionService.TryEvolve(pet);

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
        public PetActionResultDto RestPet(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.Rest();

            var evolved = _petEvolutionService.TryEvolve(pet);

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
        public PetActionResultDto GainExperience(Pet pet, int experience)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.GainExperience(experience);

            var evolved = _petEvolutionService.TryEvolve(pet);

            return new PetActionResultDto { Pet = PetMapper.ToDto(pet), Evolved = evolved };
        }
    }
}
