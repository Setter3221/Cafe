using RestaurantManagementSystem.DTO;
using RestaurantManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementSystem.Repository
{


    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task AddOrderAsync(int userId, OrderDto orderdto)
        {
            var order = new Order
            {
                UserId = userId,
                MenuItemId = orderdto.MenuItemId,
                Quantity = orderdto.Quantity,
                CreatedAt = orderdto.CreatedAt,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }





}