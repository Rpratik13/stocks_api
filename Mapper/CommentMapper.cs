using api.Dto.Comments;
using api.Models;

namespace api.Mapper;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            StockId = commentModel.StockId,
        };
    }

    public static Comment ToCommentFromCreateCommentRequestDto(
        this CreateCommentRequestDto commentDto,
        int stockId
    )
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId,
        };
    }
}
