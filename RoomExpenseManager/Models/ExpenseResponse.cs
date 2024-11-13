namespace RoomExpenseManager.Models
{
    public class ExpenseResponse
    {
        public int ExpenseId { get; set; }

        public string Item { get; set; }
        public string? Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; } 
        public int UserId { get; set; }
    }
}
