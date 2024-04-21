using RestaurantManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DTO;

namespace RestaurantManagementSystem.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITableRepository _tableRepository;
        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {


            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }
        public async Task<string> AddReservationAsync(Reservation reservation)  

        {





            //var occupiedTable = await _context.Tables.FirstOrDefaultAsync(t => t.Id == reservation.TableId && t.IsOccupied);
            //if (occupiedTable != null)
            //{
            //    return "Table already occupied";
            //}


            _context.Reservations.Add(reservation);   
            await _context.SaveChangesAsync();
            return "Table reservation successfully";
            // Mark the table as occupied
            //var table = await _context.Tables.FindAsync(reservation.TableId);
            //if (table != null)
            //{
            //    table.IsOccupied = true;
            //    await _context.SaveChangesAsync();
            //}

        }


      
        public async Task UpdateReservationAsync(Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                throw new InvalidOperationException("Reservation not found");
            }
         //   var table = await _tableRepository.GetTableByIdAsync(reservationDTO.TableId);
         //   var status = _tableRepository.UpdateTableStatusAsync(table.Id);

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

       
    }
}