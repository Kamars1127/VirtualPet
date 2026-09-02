using VirtualPet.Domain.Entities;
using VirtualPet.Domain.Enums;

namespace VirtualPet.Domain.Rules
{
    /// <summary>
    /// Pet 進化規則
    /// </summary>
    public sealed record class PetEvolutionRule(
        PetEvolutionStage FromStage,
        PetEvolutionStage ToStage,
        int RequiredLevel,
        int RequiredHappiness)
    {
        /// <summary>
        /// 判斷 Pet 是否符合此進化規則
        /// </summary>
        public bool IsSatisfiedBy(Pet pet)
        {
            return pet.EvolutionStage == FromStage &&
                   pet.Level >= RequiredLevel &&
                   pet.Status.Happiness >= RequiredHappiness;
        }
    }
}
