using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomExpenseManager.Models
{
    public class ExpenseRequest
    {
        
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Item { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        // Properties used for database interaction
        public DateTime CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; } 

    }
}
