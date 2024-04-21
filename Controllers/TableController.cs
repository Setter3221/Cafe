//using Cafe_Management_System1.DTO;
//using Cafe_Management_System1.Models;
//using Cafe_Management_System1.Repository;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Cafe_Management_System1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TableController : ControllerBase
//    {
//        private readonly ITableRepository _tableRepository;

//        public TableController(ITableRepository tableRepository)
//        {
//            _tableRepository = tableRepository;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllTables()
//        {
//            var tables = await _tableRepository.GetAllTablesAsync();
//            var tableDTOs = tables.Select(table => new TableDTO
//            {
//                Id = table.Id,
//                //Name = table.Name,
//                IsOccupied = table.IsOccupied
//            });
//            return Ok(tableDTOs);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<TableDTO>> GetTableById(int id)
//        {
//            var table = await _tableRepository.GetTableByIdAsync(id);
//            if (table == null)
//            {
//                return NotFound();
//            }

//            var tableDTO = new TableDTO
//            {
//                Id = table.Id,
//                //Name = table.Name,
//                IsOccupied = table.IsOccupied
//            };

//            return tableDTO;
//        }

//        [HttpPost]
//        [Authorize(Roles = "Admin")]
//        public async Task<ActionResult<TableDTO>> AddTable(TableDTO tableDTO)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var table = new Table
//            {
//                //name = tabledto.name,
//                IsOccupied = tableDTO.IsOccupied;
//            };

//            await _tableRepository.AddTableAsync(table);

//            return CreatedAtAction(nameof(GetTableById), new { id = table.Id }, tableDTO);
//        }

//        [HttpPut("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> UpdateTable(int id, TableDTO tableDTO)
//        {
//            if (id != tableDTO.Id)
//            {
//                return BadRequest();
//            }

//            var existingTable = await _tableRepository.GetTableByIdAsync(id);
//            if (existingTable == null)
//            {
//                return NotFound();
//            }

//            existingTable.Name = tableDTO.Name;
//            existingTable.IsOccupied = tableDTO.IsOccupied;

//            await _tableRepository.UpdateTableAsync(existingTable);

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteTable(int id)
//        {
//            var existingTable = await _tableRepository.GetTableByIdAsync(id);
//            if (existingTable == null)
//            {
//                return NotFound();
//            }

//            await _tableRepository.DeleteTableAsync(id);
//            return NoContent();
//        }
//    }
//}
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
    public class TableController : ControllerBase
    {
        private readonly ITableRepository _tableRepository;

        public TableController(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllTables()
        {
            var tables = await _tableRepository.GetAllTablesAsync();
            var tableDTOs = tables.Select(table => new TableDTO
            {
                Id = table.Id,
                //Name = table.Name,
                IsOccupied = table.IsOccupied
            });
            return Ok(tableDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TableDTO>> GetTableById(int id)
        {
            var table = await _tableRepository.GetTableByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            var tableDTO = new TableDTO
            {
                Id = table.Id,
                //Name = table.Name,
                IsOccupied = table.IsOccupied
            };

            return tableDTO;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TableDTO>> AddTable(TableDTO tableDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var table = new Table
            {
                //Name = tableDTO.Name,
                IsOccupied = tableDTO.IsOccupied
            };

            await _tableRepository.AddTableAsync(table);
            return Ok("created");

            //return CreatedAtAction(nameof(GetTableById), new { id = table.Id }, tableDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTable(int id, TableDTO tableDTO)
        {
            if (id != tableDTO.Id)
            {
                return BadRequest();
            }

            var existingTable = await _tableRepository.GetTableByIdAsync(id);
            if (existingTable == null)
            {
                return NotFound();
            }

            //existingTable.Name = tableDTO.Name;
            existingTable.IsOccupied = tableDTO.IsOccupied;

            await _tableRepository.UpdateTableAsync(existingTable);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var existingTable = await _tableRepository.GetTableByIdAsync(id);
            if (existingTable == null)
            {
                return NotFound();
            }

            await _tableRepository.DeleteTableAsync(id);
            return NoContent();
        }
    }
}