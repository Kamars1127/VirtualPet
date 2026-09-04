namespace VirtualPet.Domain.Entities
{
    /// <summary>
    /// 玩家領域實體
    /// </summary>
    public class User
    {
        private readonly List<Pet> _pets = [];

        /// <summary>
        /// 唯一識別碼
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateAt {  get; private set; }

        /// <summary>
        /// 擁有的寵物
        /// </summary>
        public IReadOnlyCollection<Pet> Pets => _pets.AsReadOnly();


        private User()
        {
            Name = string.Empty;
        }

        public User(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("User name cannot be empty.", nameof(name));
            }

            if (name.Length > 20)
            {
                throw new ArgumentException("User name cannot be longer than 20 characters.", nameof(name));
            }

            Id = Guid.NewGuid();
            Name = name;
            CreateAt = DateTime.UtcNow;
        }
    }
}
