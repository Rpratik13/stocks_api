using api.Dto.Comments;
using api.Models;

namespace api.Interface;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> Create(Comment comment);
    Task<Comment?> Update(int id, UpdateCommentRequestDto commentDto);
    Task<Comment?> Delete(int id);
}
