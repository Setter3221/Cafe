using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repository;
using RestaurantManagementSystem.DTO;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderDtos = MapOrdersToDtos(orders);
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = MapOrderToDto(order);
            return Ok(orderDto);
        }

        [HttpPost]
        [Authorize(Roles ="Customer")]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the DTO back to an Order entity
           // var order = MapDtoToOrder(orderDto);
            int userId = int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value);
            await _orderRepository.AddOrderAsync(userId,orderDto);

            // Map the created Order entity to a DTO and return
            //var createdOrderDto = MapOrderToDto(orderDto);
            //return CreatedAtAction(nameof(GetOrderById), new { id = createdOrderDto.Id }, createdOrderDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto orderDto)
        {
            if (id != orderDto.Id)
            {
                return BadRequest();
            }

            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }
            int userId = int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value);
            // Update the existing Order entity with the values from the DTO
            existingOrder.UserId = userId;
            existingOrder.MenuItemId = orderDto.MenuItemId;
            existingOrder.Quantity = orderDto.Quantity;

            await _orderRepository.UpdateOrderAsync(existingOrder);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderAsync(id);

            return NoContent();
        }

        // Helper method to map Order entities to OrderDto objects
        private IEnumerable<OrderDto> MapOrdersToDtos(IEnumerable<Order> orders)
        {
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                orderDtos.Add(MapOrderToDto(order));
            }
            return orderDtos;
        }

        // Helper method to map a single Order entity to an OrderDto object
        private OrderDto MapOrderToDto(Order order)
        {
            //int userId = int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value);
            int userId = HttpContext.User != null && HttpContext.User.HasClaim(c => c.Type == "UserId") ? int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value) : 0;
            //return new OrderDto
            //{
            //    Id = order.Id,

            //    MenuItemId = order.MenuItemId,
            //    Quantity = order.Quantity,
            //    CreatedAt = order.CreatedAt
            //};

            return new OrderDto { Id = order.Id, MenuItemId = order.MenuItemId, Quantity = order.Quantity, CreatedAt = order.CreatedAt };



        }

        // Helper method to map an OrderDto object back to an Order entity
        private Order MapDtoToOrder(OrderDto orderDto)
        {
            int userId = int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value);

            return new Order
            {
                //UserId = userId,
                MenuItemId = orderDto.MenuItemId,
                Quantity = orderDto.Quantity,
                CreatedAt = orderDto.CreatedAt
            };
        }
    }
}
