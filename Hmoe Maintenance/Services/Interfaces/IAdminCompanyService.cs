using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IAdminCompanyService
    {
        Task<bool> ApproveCompanyCreate(int notid);

        Task<bool> ApproveCompanyUpdate(int notid);

        Task<bool> RejectCompanyCreate(int notificationId);


        Task<bool> RejectCompanyUpdate(int notificationId);
        
    }
}


