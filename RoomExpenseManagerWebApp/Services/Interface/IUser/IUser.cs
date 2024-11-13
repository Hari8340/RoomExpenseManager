using RoomExpenseManagerWebApp.Models;

namespace RoomExpenseManagerWebApp.Services.Interface.IUser
{
    public interface IUser
    {

        Task<UserRequest> CreateUserAsync(UserRequest user);

        Task<UserRequest> UpdateUserAsync(int userId, UserRequest user);
        
        Task<List<UserResponse>> GetAllUsersAsync();

        Task<UserResponse> GetUserByIdAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
        


    }
}
