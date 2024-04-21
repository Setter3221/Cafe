using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models

{
    public class Table
    {
        public int Id { get; set; }
       

        [Required(ErrorMessage = "Name is required")]
        //public string Name { get; set; }

        public bool IsOccupied { get; set; }

        // Navigation properties
        //added extra//added extra
        //public Payment Payment { get; set; }
        //public ICollection<Reservation> Reservations { get; set; }
       
    }
}
