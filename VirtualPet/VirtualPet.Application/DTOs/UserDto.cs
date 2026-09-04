namespace VirtualPet.Application.DTOs
{
    /// <summary>
    /// User 資料傳輸物件
    /// </summary>
    public sealed class UserDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public DateTime CreateAt { get; init; }
    }
}
