using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomExpenseManager.Models
{
    public class User
    {
        [Key] // Marks the property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public byte[] ImageData { get; set; } // Store the binary content of the image
        public byte[] AadharPdfData { get; set; }

        // Properties used for database interaction
        public DateTime DOB { get; set; } = DateTime.UtcNow.Date;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
