namespace RoomExpenseManagerWebApp.Models
{
    public class Expense
    {
       
        public int UserId { get; set; }
        public string? Item { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
       
    }
    public class DashboardViewModel
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
