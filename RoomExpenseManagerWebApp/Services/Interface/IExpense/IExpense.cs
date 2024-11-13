using RoomExpenseManagerWebApp.Models;

namespace RoomExpenseManagerWebApp.Services.Interface.IExpense
{
    public interface IExpense
    {
        Task<Expense> CreateExpenseAsync(Expense expense);

        Task<Expense> UpdateExpenseAsync(int expenseId, Expense user);

        Task<IEnumerable<ExpenseResponse>> GetAllExpenseAsync();
        Task<ExpenseResponse> GetExpenseByIdAsync(int expenseId);
        Task<bool> DeleteExpenseAsync(int expenseId);
    }
}
