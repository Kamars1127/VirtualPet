using VirtualPet.Domain.Enums;

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

        /// <summary>
        /// 所屬 Pet Id
        /// </summary>
        public Guid PetId { get; private set; }

        /// <summary>
        /// 操作類型
        /// </summary>
        public PetHistoryType ActionType { get; private set;  }

        /// <summary>
        /// 操作說明
        /// </summary>
        public string Description {  get; private set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateAt { get; private set; }

        /// <summary>
        /// 所屬 Pet
        /// </summary>
        public Pet pet { get; private set; } = null;

        private PetHistory()
        {
            Description = string.Empty;
        }

        public PetHistory(Guid petId, PetHistoryType actionType, string description)
        {
            if(petId == Guid.Empty)
            {
                throw new ArgumentException("Pet id cannot be empty.", nameof(petId));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("History description cannot be empty.", nameof(description));
            }

            Id = Guid.NewGuid();
            PetId = petId;
            ActionType = actionType;
            Description = description;
            CreateAt = DateTime.UtcNow;
        }
    }
}
