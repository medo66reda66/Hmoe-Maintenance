using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ITechnicianerviceSesrvice
    {
        Task<List<ShowTechnicianServiceResponse>> GetAllTechnicianService();


        Task<ShowTechnicianServiceResponse?> GetTechnicianServiceById(int id);
        
            Task<TechnicianService> createTechnicianervice(TechnicianServiceRequest technicianService,string tecid);

         Task<TechnicianService> UpdateTechnicianervice(int id , TechnicianServiceRequest technicianService);

         Task<bool> DeleteTechnicianervice(int id ,TechnicianService technicianService);
     
    }
}
