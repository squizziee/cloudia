using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IReportService
    {
        public Task<Report?> GetReport(int id);
        public Task<Report> AddReport(int senderId, int postid, int violationId);
    }
}
