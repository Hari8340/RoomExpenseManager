using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomExpenseManager.Models
{
    public class Expense
    {
        [Key] // Marks the property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }

        [Required]
        public int UserId { get; set; }  // Foreign key to User entity

        [Required]
        public string Item { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        // Properties used for database interaction
        public DateTime CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; } 

        
        public virtual User User { get; set; }
    }

}
