using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;

namespace RoomExpenseManager.Implementation
{
    public class UserService:IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _userRepository.GetAllAsync();
        public async Task<User> GetUserByIdAsync(int id) => await _userRepository.GetByIdAsync(id);
        public async Task AddUserAsync(User user) => await _userRepository.AddAsync(user);
        public async Task UpdateUserAsync(User user) => await _userRepository.UpdateAsync(user);
        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        
    }
}
