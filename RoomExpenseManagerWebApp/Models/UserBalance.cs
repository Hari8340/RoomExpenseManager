namespace RoomExpenseManagerWebApp.Models
{
    public class UserBalance
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Balance { get; set; } // Positive for amount owed, negative for amount due
        public List<Settlement> Settlements { get; set; } = new List<Settlement>();
    }
}
