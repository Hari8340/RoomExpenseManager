using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RoomExpenseManager.Models
{
    public class Login
    {
        public string Id { get; set; }
        public string Password { get; set; }
        
        public int UserId { get; set; }
        
    }
}
