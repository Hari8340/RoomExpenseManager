namespace RoomExpenseManagerWebApp.Models
{
    public class Settlement
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
