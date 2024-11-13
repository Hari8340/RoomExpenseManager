using RoomExpenseManager.Models;

namespace RoomExpenseManager.Interfaces
{
    public interface ILogin
    {
        Task<IEnumerable<Login>> GetAllLogins();
        Task<Login> GetLoginById(string userId);
    }
}
