using RestaurantManagementSystem.DTO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase

    {

        private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;

        public ReservationController(IReservationRepository reservationRepository, ITableRepository tableRepository)
        {
            _reservationRepository = reservationRepository;
            _tableRepository = tableRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();


            int userId = HttpContext.User != null && HttpContext.User.HasClaim(c => c.Type == "UserId") ? int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value) : 0;
            var ReservationDTOs = reservations.Select(r => new ReservationDTO
            {
                Id = r.Id,
                //UserId = r.UserId,
                TableId = r.TableId,
                DateTime = r.DateTime
            });
            return Ok(ReservationDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDTO>> GetReservationById(int id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
           

            var ReservationDTO = new ReservationDTO
            {
                Id = reservation.Id,
                //UserId = reservation.UserId,
                TableId = reservation.TableId,
                DateTime = reservation.DateTime
            };
            return Ok(ReservationDTO);
        }

        [HttpPost]
        [Authorize(Roles="Customer")]
        public async Task<IActionResult> AddReservation(ReservationDTO reservationDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            

            int UserID = int.Parse(HttpContext.User.FindFirst(t => t.Type == "UserId").Value);
            

            var table = await _tableRepository.GetTableByIdAsync(reservationDTO.TableId);
            if (table == null)
            {
                return NotFound();
            }

           
            if(table.IsOccupied == true)
            {
                return BadRequest("The table is already occupied.");
            }
            var reservation = new Reservation
            {
                UserId = UserID,
                TableId = reservationDTO.TableId,
                DateTime = reservationDTO.DateTime
            };
            var result = await _reservationRepository.AddReservationAsync(reservation);
            //if (result == null)
            //{
            //    return NotFound();
            //}

            var status =  _tableRepository.UpdateTableStatusAsync(table.Id);
            Console.WriteLine(table.IsOccupied);

            //if (status == null || !status.IsSuccess)
            //{
            //    return StatusCode(500, "Failed to update table status.");
            //}

            //reservationDTO.Id = reservation.Id;

            return Ok(result);
            //return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, ReservationDTO);
            //reservation.Table = table;
            //table.IsOccupied = true;
            //_context.Entry(table).State = EntityState.Modified;
            //if (reservation == null)
            //{
            //    return NotFound();
            //}

            //if (reservation.Table.IsOccupied)
            //{
            //    return BadRequest("The table is already occupied.");
            //}


            //var result =  await _reservationRepository.AddReservationAsync(reservation);
            //var status=_tableRepository.UpdateTableStatusAsync(reservation.TableId);

            //ReservationDTO.Id = reservation.Id;

            //return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, ReservationDTO ReservationDTO)
        {
            if (id != ReservationDTO.Id)
            {
                return BadRequest();
            }




            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            //reservation.UserId = ReservationDTO.UserId;
            reservation.TableId = ReservationDTO.TableId;
            reservation.DateTime = ReservationDTO.DateTime;

            await _reservationRepository.UpdateReservationAsync(reservation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            
            
                var reservation = await _reservationRepository.GetReservationByIdAsync(id);

                if (reservation == null)
                {
                    return NotFound();
                }

               // var table = await _tableRepository.GetTableByIdAsync(reservationDTO.TableId);
                // var status = _tableRepository.UpdateTableStatusAsync(table.Id);

                await _reservationRepository.DeleteReservationAsync(id);

                return NoContent();
            
        }
    }



}