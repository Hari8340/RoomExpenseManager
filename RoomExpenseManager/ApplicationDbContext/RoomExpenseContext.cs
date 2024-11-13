using Microsoft.EntityFrameworkCore;
using RoomExpenseManager.Models;

namespace RoomExpenseManager.ApplicationDbContext
{
    public class RoomExpenseContext : DbContext
    {
        public RoomExpenseContext(DbContextOptions<RoomExpenseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Login> Logins { get; set; }

    }
}
