using RestaurantManagementSystem.DTO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemController(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAllMenuItems()
        {
            var menuItems = await _menuItemRepository.GetAllMenuItemsAsync();
            var menuItemDtos = menuItems.Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price
            }).ToList();
            return Ok(menuItemDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemDto>> GetMenuItemById(int id)
        {
            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            var menuItemDto = new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price
            };
            return Ok(menuItemDto);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<MenuItemDto>> AddMenuItem(MenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price
            };
            await _menuItemRepository.AddMenuItemAsync(menuItem);

            menuItemDto.Id = menuItem.Id;
            return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItem.Id }, menuItemDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItemDto menuItemDto)
        {
            if (id != menuItemDto.Id)
            {
                return BadRequest();
            }

            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem.Name = menuItemDto.Name;
            menuItem.Description = menuItemDto.Description;
            menuItem.Price = menuItemDto.Price;

            await _menuItemRepository.UpdateMenuItemAsync(menuItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            await _menuItemRepository.DeleteMenuItemAsync(id);

            return NoContent();
        }
    }
}