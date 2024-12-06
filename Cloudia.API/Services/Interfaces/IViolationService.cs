using Cloudia.API.Entities;

namespace Cloudia.API.Services.Interfaces
{
    public interface IViolationService
    {
        Task<Violation?> GetViolation(int id);
        Task<Violation> AddViolation(string name, string description, int banDays);
        Task<Violation> UpdateViolation(int violationId, string name, string description, int banDays);
        Task<bool> DeleteViolation(int id);
    }
}
