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
            _petEvolutionService = petEvolutionService ?? throw new ArgumentNullException(nameof(petEvolutionService))
        }

        /// <summary>
        /// 建立 Pet
        /// </summary>
        /// <param name="name">寵物名稱</param>
        /// <param name="species">寵物種類</param>
        /// <returns></returns>
        public Pet CreatePet(string name, PetSpecies species)
        {
            return new Pet(name, species);
        }

        /// <summary>
        /// 餵食 Pet
        /// </summary>
        /// <returns>是否發生進化</returns>
        public bool FeedPet(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.Feed();

            return _petEvolutionService.TryEvolve(pet);
        }

        /// <summary>
        /// 陪 Pet 玩
        /// </summary>
        /// <returns>是否發生進化</returns>
        public bool PlayWithPet(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.Play();

            return _petEvolutionService.TryEvolve(pet);
        }

        /// <summary>
        /// 讓 Pet 休息
        /// </summary>
        /// <returns>是否發生進化</returns>
        public bool RestPet(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.Rest();

            return _petEvolutionService.TryEvolve(pet);
        }

        /// <summary>
        /// 增加 Pet 經驗值
        /// </summary>
        /// <param name="experience">增加的經驗值</param>
        /// <returns>是否發生進化</returns>
        public bool GainExperience(Pet pet, int experience)
        {
            ArgumentNullException.ThrowIfNull(pet);

            pet.GainExperience(experience);

            return _petEvolutionService.TryEvolve(pet);
        }
    }
}
