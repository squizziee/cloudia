using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IUserProfileService
    {
        public Task<UserProfile?> GetUserProfile(int userId);
        public Task<UserProfile> UpdateUserProfile(int userId, string firstName, string lastName, string? avatarUrl, string? location, string? biography, int? age);
        public Task<List<UserProfile>> GetSubscribers(int userId);
        public Task<List<UserProfile>> GetSubscriptions(int userId);
        public Task<bool> SubscribeTo(int userId, int subscribeToProfileId);
        public Task<bool> UnsubscribeFrom(int userId, int subscribeToProfileId);
        public Task<List<(Post post, List<PostAttachment>? attachments, List<Comment>? comments, List<Like>? likes)>> GetFeed(int userId);

    }
}
