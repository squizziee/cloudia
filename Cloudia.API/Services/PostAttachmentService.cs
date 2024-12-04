using Cloudia.API.Data;
using Cloudia.API.Entities;
using Cloudia.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Cloudia.API.Services
{
    public class PostAttachmentService : IPostAttachmentService
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<PostAttachmentService> _logger;
        private readonly IWebHostEnvironment _environment;

        public PostAttachmentService(IApplicationContext context, ILogger<PostAttachmentService> logger, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._logger = logger;
            _environment = webHostEnvironment;
        }

        private async Task<string> SaveAttachment(IFormFile attachment)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "img");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"file{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";

            _logger.LogWarning($"New file: {uniqueFileName}");

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            _logger.LogWarning($"New file path: {filePath}");

            var fileInfo = new FileInfo(filePath);
            using var fileStream = fileInfo.Create();
            await attachment.CopyToAsync(fileStream);

            return $"https://localhost:5001/img/{uniqueFileName}";
        }

        public async void CreatePostAttachments(int postId, IFormFileCollection attachments)
        {
            using var connection = new NpgsqlConnection(_context.GetConnectionString());
            await connection.OpenAsync();

            foreach (var attachment in attachments)
            {
                var url = await this.SaveAttachment(attachment);

                var command = new NpgsqlCommand("CALL add_post_attachment(@post_id_new, @source_url_new)", connection);
                //command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@post_id_new", postId);
                command.Parameters.AddWithValue("@source_url_new", url);

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();
        }

        public List<PostAttachment> GetPostAttachments(int postId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAttachments(int postId, params PostAttachment[] newPostAttachments)
        {
            throw new NotImplementedException();
        }
    }
}
