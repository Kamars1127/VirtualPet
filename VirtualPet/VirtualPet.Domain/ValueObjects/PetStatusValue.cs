

namespace VirtualPet.Domain.ValueObjects
{
    public sealed record PetStatusValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 100;

        /// <summary>
        /// 飽食度
        /// </summary>
        public int Satiety { get; }

        /// <summary>
        /// 心情值
        /// </summary>
        public int Happiness { get; }

        public int Energy { get; }

        public PetStatusValue(int satiety, int happiness, int energy)
        {
            Satiety = ValidateValue(satiety);
            Happiness = ValidateValue(happiness);
            Energy = ValidateValue(energy);
        }

        /// <summary>
        /// 變更寵物狀態數值
        /// </summary>
        /// <param name="satiety">飽食度變化量</param>
        /// <param name="happiness">心情值變化量</param>
        /// <param name="energy">體力值變化量</param>
        /// <returns>新的寵物狀態數值</returns>
        public PetStatusValue Change(int satiety = 0, int happiness = 0, int energy = 0)
        {
            return new PetStatusValue(
                satiety: Clamp(Satiety + satiety),
                happiness: Clamp(Happiness + happiness),
                energy: Clamp(Energy + energy));
        }

        /// <summary>
        /// 驗證狀態值是否介於允許範圍內
        /// </summary>
        private static int ValidateValue(int value)
        {
            if(value < MinValue || value > MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"Pet status value must be between {MinValue} and {MaxValue}.");
            }

            return value;
        }

        /// <summary>
        /// 將狀態數值限制在允許範圍內
        /// </summary>
        private static int Clamp(int value)
        {
            return Math.Clamp(value, MinValue, MaxValue);
        }
    }
}
