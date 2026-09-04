using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Repositories
{
    /// <summary>
    /// Pet 資料存取介面
    /// </summary>
    public interface IPetRepository
    {
        /// <summary>
        /// 依 Id 取得 Pet
        /// </summary>
        /// <returns></returns>
        Task<Pet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得所有 Pet
        /// </summary>
        Task<IReadOnlyList<Pet>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增 Pet
        /// </summary>
        Task AddAsync(Pet pet, CancellationToken cancellationToken = default);

        /// <summary>
        /// 儲存目前的資料變更
        /// </summary>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
