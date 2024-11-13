namespace RoomExpenseManager.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> GetUserIdAsync(string userId);
        Task<bool> DeleteAsync(int id);
    }
}
