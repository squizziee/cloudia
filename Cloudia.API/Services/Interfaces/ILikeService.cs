namespace Cloudia.API.Services.Interfaces
{
    public interface ILikeService
    {
        public Task<bool> AddLike(int userId, int postId);
        public Task<bool> RemoveLike(int userId, int postId);
    }
}
