using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RoomExpenseManager.Models
{
    public class UserRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        
        public IFormFile? Image { get; set; }

       
        public IFormFile? AadharPdf { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DOB { get; set; } = DateTime.UtcNow.Date;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    }
    public class UserDoc
    {
       
        public byte[] ImageData { get; set; } // Store the binary content of the image
       
        public byte[] AadharPdfData { get; set; }

    }
}
