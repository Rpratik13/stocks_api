using api.Dto.Stocks;
using api.Helpers;
using api.Interface;
using api.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;

    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        var stocks = await _stockRepository.GetAllAsync(query);

        var stockDto = stocks.Select(stock => stock.ToStockDto());

        return Ok(stockDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockRequest)
    {
        var stock = stockRequest.ToStockFromCreateStockRequestDto();

        await _stockRepository.CreateAsync(stock);

        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateStockRequestDto stockRequest
    )
    {
        var updatedStock = await _stockRepository.UpdateAsync(id, stockRequest);

        if (updatedStock == null)
        {
            return NotFound();
        }

        return Ok(updatedStock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _stockRepository.DeleteAsync(id);

        return NoContent();
    }
}
