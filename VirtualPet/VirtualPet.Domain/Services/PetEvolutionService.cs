using VirtualPet.Domain.Entities;
using VirtualPet.Domain.Enums;
using VirtualPet.Domain.Rules;

namespace VirtualPet.Domain.Services
{
    /// <summary>
    /// 處理 Pet 進化規則
    /// </summary>
    public sealed class PetEvolutionService
    {
        public static readonly IReadOnlyList<PetEvolutionRule> Rules =
            [
                new(
                    FromStage: PetEvolutionStage.Egg,
                    ToStage: PetEvolutionStage.Baby,
                    RequiredLevel: 2,
                    RequiredHappiness: 0),

                new(
                    FromStage: PetEvolutionStage.Baby,
                    ToStage: PetEvolutionStage.Child,
                    RequiredLevel: 5,
                    RequiredHappiness: 60),

                new(
                    FromStage: PetEvolutionStage.Child,
                    ToStage: PetEvolutionStage.Adult,
                    RequiredLevel: 10,
                    RequiredHappiness: 80)
            ];

        /// <summary>
        /// 嘗試讓 Pet 進化
        /// </summary>
        /// <param name="pet"></param>
        /// <returns>g是否至少發生一次進化</returns>
        public bool TryEvolve(Pet pet)
        {
            ArgumentNullException.ThrowIfNull(pet);

            var evolved = false;

            /* 處理 Pet 一次獲得大量經驗值的情況 */
            while (true)
            {
                var rule = Rules.FirstOrDefault(rule => rule.IsSatisfiedBy(pet));

                if(rule is null)
                {
                    return evolved;
                }

                pet.EvolveTo(rule.ToStage);

                evolved = true;
            }
        }
    }
}
