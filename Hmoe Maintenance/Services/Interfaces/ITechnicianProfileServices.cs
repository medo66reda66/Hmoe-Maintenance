using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ITechnicianProfileServices
    {
        Task<TechnicianProfile> CreateTechniciaProfile(CreateTechnicianProfileRequest request, string userId);
        Task<TechnicianProfile?> UpdateTechniciaProfile(int id, UpdateTechniciaProfileRequest request);
        Task<IEnumerable<TechnincianProfileResponse>> GetAllTechnicianProfiles();
        Task<TechnincianProfileResponse?> GetTechnicianProfileById(int id);
        Task<bool> DeleteTechnicianProfile(int id);
    }
}
