using Microsoft.EntityFrameworkCore;
using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;

namespace RoomExpenseManager.Implementation
{
    public class ExpenseService:IExpenseService 
    {
        private readonly IGenericRepository<Expense> _expenseRepository;

        public ExpenseService(IGenericRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync() => await _expenseRepository.GetAllAsync();
        public async Task<Expense> GetExpenseByIdAsync(int id) => await _expenseRepository.GetByIdAsync(id);
        public async Task AddExpenseAsync(Expense expense) => await _expenseRepository.AddAsync(expense);
        public async Task UpdateExpenseAsync(Expense expense) => await _expenseRepository.UpdateAsync(expense);
        public async Task DeleteExpenseAsync(int id) => await _expenseRepository.DeleteAsync(id);
        
    }
}
