using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repository
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(  int id);
        Task AddTableAsync(Table table);



        Task UpdateTableStatusAsync(int id);
        Task UpdateTableAsync(Table table);
        Task DeleteTableAsync(int id);
    }
}
