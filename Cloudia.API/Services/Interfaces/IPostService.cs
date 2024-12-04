using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IPostService
    {
        List<Post> GetPosts(string filter);
        Task<Post?> GetPost(int id);
        Task<Post> AddPost(int userProfileId, string textContent, IFormFileCollection attachments);
        void UpdatePost(int id);
        void DeletePost(int id);
    }
}
