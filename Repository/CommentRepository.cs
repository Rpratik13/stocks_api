using api.Data;
using api.Dto.Comments;
using api.Interface;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return null;
        }

        return comment;
    }

    public async Task<Comment> Create(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> Update(int id, UpdateCommentRequestDto commentDto)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        if (comment == null)
        {
            return null;
        }

        comment.Title = commentDto.Title;
        comment.Content = commentDto.Content;

        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment?> Delete(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        if (comment == null)
        {
            return null;
        }

        _context.Remove(comment);
        await _context.SaveChangesAsync();

        return comment;
    }
}
