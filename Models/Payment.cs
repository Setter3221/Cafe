using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "DateTime is required")]
        public DateTime DateTime { get; set; }

        // Navigation properties
        
        public User User { get; set; }
    }

}