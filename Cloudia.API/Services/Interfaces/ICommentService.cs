using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment?> GetComment(int id);
        Task<Comment> AddComment(int userId, int postId, string textContent);
        Task<Comment> UpdateComment(int commentId, string newtextContent);
        Task<bool> DeleteComment(int id);
    }
}
