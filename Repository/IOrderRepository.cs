using RestaurantManagementSystem.DTO;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repository

{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task AddOrderAsync(int userId,OrderDto order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}