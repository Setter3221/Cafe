using RestaurantManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Runtime.ExceptionServices;

namespace RestaurantManagementSystem.Repository
{
    public class TableRepository : ITableRepository
    {
        private readonly ApplicationDbContext _context;

        public TableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table> GetTableByIdAsync(int id)
        {
            return await _context.Tables.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddTableAsync(Table table)
        {

            table.IsOccupied = false;
             _context.Tables.Add(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTableAsync(Table table)
        {

            _context.Entry(table).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        //public async Task UpdateTableStatusAsync(int id)
        //{

        //    var tables = await GetTableByIdAsync(id);

        //    tables.IsOccupied= false;
        //    await _context.SaveChangesAsync();     

        //}





        public async Task UpdateTableStatusAsync(int id)
        {

            var tables =_context.Tables.FirstOrDefault(x=>x.Id == id);
            //tables.IsOccupied = true;

            //Console.WriteLine(tables.IsOccupied);
            //await _context.SaveChangesAsync();
            if (tables != null)
            {
                tables.IsOccupied = true;
                // Console.WriteLine(tables.IsOccupied);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Table not found.");
            }

        }




        public async Task DeleteTableAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                throw new InvalidOperationException("Table not found");
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
        }
    }
}
