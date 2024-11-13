using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;

namespace RoomExpenseManager.Implementation
{
    public class LoginService : ILogin
    {
        private readonly IGenericRepository<Login> _loginRepository;

        public LoginService(IGenericRepository<Login> loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public async Task<IEnumerable<Login>> GetAllLogins() => await _loginRepository.GetAllAsync();

        public async Task<Login> GetLoginById(string userId)=> await _loginRepository.GetUserIdAsync(userId);

    }
}
