namespace RoomExpenseManagerWebApp.Models
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }
        public string Item { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
    }
}
