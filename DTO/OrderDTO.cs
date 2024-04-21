namespace RestaurantManagementSystem.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}