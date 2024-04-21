using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models
{
    public class Order
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "MenuItemId is required")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}