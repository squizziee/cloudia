using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPosts(string filter);
        Task<List<(Post post, List<PostAttachment>? attachments, List<Comment>? comments, List<Like>? likes)>> GetUserPosts(int userId);
        Task<Post?> GetPost(int id);
        Task<(Post? post, List<PostAttachment>? attachments, List<Comment>? comments, List<Like>? likes)> GetFullPost(int id);
        Task<Post> AddPost(int userId, string textContent, IFormFileCollection attachments);
        Task<Post> UpdatePost(int userId, string textContent, IFormFileCollection attachments);
        void DeletePost(int id);
    }
}
