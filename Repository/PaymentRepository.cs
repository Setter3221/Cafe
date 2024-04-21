using RestaurantManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementSystem.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }


        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        
        public async Task AddPaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);

           
            await _context.SaveChangesAsync();


      

        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found");
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }

}
