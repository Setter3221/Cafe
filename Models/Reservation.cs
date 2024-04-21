using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "TableId is required")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "DateTime is required")]
        public DateTime DateTime { get; set; }

        //Navigation properties
        public User User { get; set; }
       // public Table Table { get; set; }
    }
}