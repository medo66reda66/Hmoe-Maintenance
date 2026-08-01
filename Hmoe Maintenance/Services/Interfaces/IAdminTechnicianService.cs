using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IAdminTechnicianService
    {
        Task<bool> ApproveTechnicienCreate(int notifId);

        Task<bool> RejectTechnicienCreate(int notifId);

        Task<bool> ApproveTechnicianUpdate(int notifId);


        Task<bool> RejectTechnicianUpdate(int notifId);
        
    }
}
