using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ITechnicianerviceSesrvice
    {
        Task<PaginationResponse<ShowTechnicianServiceResponse>> GetAllTechnicianService(FilterTechnicianServiceRequest filter, int page);


        Task<ShowTechnicianServiceResponse?> GetTechnicianServiceById(int id);
        
            Task<TechnicianService> createTechnicianervice(TechnicianServiceRequest technicianService,string tecid);

         Task<TechnicianService> UpdateTechnicianervice(int id , TechnicianServiceRequest technicianService);

         Task<bool> DeleteTechnicianervice(int id ,TechnicianService technicianService);
     
    }
}
