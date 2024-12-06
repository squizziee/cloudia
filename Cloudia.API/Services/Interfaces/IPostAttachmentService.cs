using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IPostAttachmentService
    {
        public Task<List<PostAttachment>> GetPostAttachments(int postId);
        public void CreatePostAttachments(int postId, IFormFileCollection attachments);
        public void UpdatePostAttachments(int postId, IFormFileCollection newAttachments);
    }
}
