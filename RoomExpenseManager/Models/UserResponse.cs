namespace RoomExpenseManager.Models
{
    public class UserResponse
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime DOB { get; set; } = DateTime.UtcNow.Date;
        public byte[] ImageData { get; set; }
        public string ImageBase64 => Convert.ToBase64String(ImageData ?? new byte[0]);
    }
}
