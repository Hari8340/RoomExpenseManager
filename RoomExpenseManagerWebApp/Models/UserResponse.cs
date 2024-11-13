namespace RoomExpenseManagerWebApp.Models
{
   
        public class UserResponse
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public byte[] ImageData { get; set; }
            public byte[] AadharPdfData { get; set; }

        // Properties to convert byte arrays to Base64 strings
        public DateTime DOB { get; set; } = DateTime.UtcNow.Date;
        public string ImageBase64 => Convert.ToBase64String(ImageData ?? new byte[0]);
            public string AadharPdfBase64 => Convert.ToBase64String(AadharPdfData ?? new byte[0]);
        }
   
}
