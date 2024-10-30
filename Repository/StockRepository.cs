using api.Data;
using api.Dto.Stocks;
using api.Helpers;
using api.Interface;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);

        await _context.SaveChangesAsync();

        return stock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

        if (existingStock == null)
        {
            return null;
        }

        _context.Stocks.Remove(existingStock);

        await _context.SaveChangesAsync();

        return existingStock;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _context.Stocks.Include(stock => stock.Comments).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Company))
        {
            stocks = stocks.Where(stock => stock.Company.Contains(query.Company));
        }

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(stock => stock.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending
                    ? stocks.OrderByDescending(stock => stock.Symbol)
                    : stocks.OrderBy(stock => stock.Symbol);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int Id)
    {
        return await _context
            .Stocks.Include(stock => stock.Comments)
            .FirstOrDefaultAsync(stock => stock.Id == Id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stock)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

        if (existingStock == null)
        {
            return null;
        }

        existingStock.Company = stock.Company;
        existingStock.Industry = stock.Industry;
        existingStock.LastDiv = stock.LastDiv;
        existingStock.MarketCap = stock.MarketCap;
        existingStock.Purchase = stock.Purchase;
        existingStock.Symbol = stock.Symbol;

        await _context.SaveChangesAsync();

        return existingStock;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.Stocks.AnyAsync(stock => stock.Id == id);
    }
}
