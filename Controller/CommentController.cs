using api.Dto.Comments;
using api.Interface;
using api.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller;

[Route("api/comments")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;

    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comments = await _commentRepository.GetAllAsync();

        return Ok(comments.Select(comment => comment.ToCommentDto()));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create(
        [FromRoute] int stockId,
        [FromBody] CreateCommentRequestDto commentDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _stockRepository.Exists(stockId))
        {
            return BadRequest("Stock does not exist");
        }

        var commentModel = commentDto.ToCommentFromCreateCommentRequestDto(stockId);

        await _commentRepository.Create(commentModel);

        return CreatedAtAction(
            nameof(GetById),
            new { id = commentModel.Id },
            commentModel.ToCommentDto()
        );
    }

    [HttpPut("{commentId:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int commentId,
        [FromBody] UpdateCommentRequestDto commentDto
    )
    {
        var comment = await _commentRepository.Update(commentId, commentDto);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var comment = await _commentRepository.Delete(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment);
    }
}
