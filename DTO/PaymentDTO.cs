namespace RestaurantManagementSystem.DTO
{

    public class PaymentDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; }
    }
}