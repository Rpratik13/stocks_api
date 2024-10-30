using System.ComponentModel.DataAnnotations;
using api.Dto.Comments;

namespace api.Dto.Stocks;

public class StockDto
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public decimal Purchase { get; set; }
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }

    public List<CommentDto> Comments { get; set; } = [];
}
