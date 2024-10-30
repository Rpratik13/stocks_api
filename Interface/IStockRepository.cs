using api.Dto.Stocks;
using api.Helpers;
using api.Models;

namespace api.Interface;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stock);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stock);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> Exists(int id);
}
