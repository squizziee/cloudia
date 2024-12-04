using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IPostAttachmentService
    {
        public List<PostAttachment> GetPostAttachments(int postId);
        public void CreatePostAttachments(int postId, IFormFileCollection attachments);
        public void UpdateAttachments(int postId, params PostAttachment[] newPostAttachments);
    }
}
