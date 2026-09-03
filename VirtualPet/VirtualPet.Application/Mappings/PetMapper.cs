using VirtualPet.Application.DTOs;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Mappings
{
    /// <summary>
    /// Pet Domain Entity 與 DTO 的轉換
    /// </summary>
    public static class PetMapper
    {
        /// <summary>
        /// 將 Pet Entity 轉換成 PetDto
        /// </summary>
        public static PetDto ToDto(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            return new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Species = pet.Species,
                EvolutionStage = pet.EvolutionStage,
                State = pet.State,
                Level = pet.Level,
                Experience = pet.Experience,
                Satiety = pet.Status.Satiety,
                Happiness = pet.Status.Happiness,
                Energy = pet.Status.Energy,
                CreateAt = pet.CreateAt
            };
        }
    }
}
