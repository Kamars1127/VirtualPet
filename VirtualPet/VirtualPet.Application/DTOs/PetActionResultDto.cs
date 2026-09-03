

namespace VirtualPet.Application.DTOs
{
    /// <summary>
    /// Pet 行為執行結果
    /// </summary>
    public sealed class PetActionResultDto
    {
        /// <summary>
        /// 操作後的 Pet資料
        /// </summary>
        public required PetDto Pet { get; init; }

        /// <summary>
        /// 此次操作是否發生進化
        /// </summary>
        public bool Evolved {  get; init; }
    }
}
