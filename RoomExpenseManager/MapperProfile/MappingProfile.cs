using AutoMapper;
using RoomExpenseManager.Models;


namespace RoomExpenseManager.MapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>();

            CreateMap<Expense, ExpenseResponse>();
            CreateMap<ExpenseRequest, Expense>();
        }
    }
}
