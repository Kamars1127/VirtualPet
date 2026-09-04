namespace VirtualPet.Domain.Entities
{
    /// <summary>
    /// Pet 操作歷史紀錄
    /// </summary>
    public class PetHistory
    {
        /// <summary>
        /// 唯一識別碼
        /// </summary>
        public Guid Id { get; private set; }

        public Guid PetId { get; private set; }
    }
}
