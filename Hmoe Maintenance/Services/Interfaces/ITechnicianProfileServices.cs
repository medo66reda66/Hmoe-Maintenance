using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ITechnicianProfileServices
    {
        Task<TechnincianProfileResponse?> GetMyTechnicianProfile(string techid);
        Task<TechnicianProfile> CreateTechniciaProfile(CreateTechnicianProfileRequest request, string userId);
        Task<TechnicianProfile?> UpdateTechniciaProfile(int id, UpdateTechniciaProfileRequest request);
        Task<bool> DeleteTechnicianProfile(int id);
        Task<List<ShowReviewsResponse>> GetReviews(
                 int techId,
                 FilterReviewRequest filter);
    }
}
