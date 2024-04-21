namespace RestaurantManagementSystem.DTO
{
    public class ReservationDTO
    {
        public int Id { get; set; }

        //public int UserId { get; set; }

        public int TableId { get; set; }

        public DateTime DateTime { get; set; }
    }
}