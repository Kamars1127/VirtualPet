using VirtualPet.Domain.Enums;

namespace VirtualPet.Application.DTOs
{
    /// <summary>
    /// Pet 資料傳輸物件
    /// </summary>
    public sealed class PetDto
    {
        /// <summary>
        /// 唯一識別碼
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// 所屬玩家 Id
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// 種類
        /// </summary>
        public PetSpecies Species { get; init; }

        /// <summary>
        /// 進化階段
        /// </summary>
        public PetEvolutionStage EvolutionStage { get; init; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        public PetState State { get; init; }

        /// <summary>
        /// 等級
        /// </summary>
        public int Level {  get; init; }

        /// <summary>
        /// 經驗值
        /// </summary>
        public int Experience {  get; init; }

        /// <summary>
        /// 飽食度
        /// </summary>
        public int Satiety {  get; init; }

        /// <summary>
        /// 心情值
        /// </summary>
        public int Happiness {  get; init; }

        /// <summary>
        /// 體力值
        /// </summary>
        public int Energy {  get; init; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateAt { get; init; }

        /// <summary>
        /// 最後一次狀態時間更新
        /// </summary>
        public DateTime LastStatusUpdateAt { get; init; }
    }
}
