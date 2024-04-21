using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.DTO
{
    public class TableDTO
    {

              

            public int Id { get; set; }

            //[Required(ErrorMessage = "Name is required")]
            //public string Name { get; set; }

            public bool IsOccupied { get; set; }
        }

    }

