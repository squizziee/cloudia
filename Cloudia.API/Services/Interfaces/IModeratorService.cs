namespace Cloudia.API.Services.Interfaces
{
    public interface IModeratorService
    {
        public Task<bool> DeletePost(int postId);
    }
}
