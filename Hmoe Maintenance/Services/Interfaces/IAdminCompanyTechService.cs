using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IAdminCompanyTechService
    {
        Task<Notification> Sendnotification(string adminId, CreateSendNotificationRequest sendNotificationRequest);
        Task<PaginationResponse<Notification,FilternotificationRespons>?> GetNotification(string adminid, FilternotificationRequest filternotification,int page);

        Task<Notification?> GetNotificationBYid(string adminid, int notid);
        Task<PaginationResponse<Company, FiltercompanyResponse>> GetAllCompany(FiltercompanyReqest filtercompany, int page);
       
        Task<Company> GetCompanyById(int companyId);

        Task<PaginationResponse<CompanyAreaResponse, FiltercompanyResponse>> GetAllCompanyCoverageAreas(FiltercompanyReqest filtercompany, int page);

        Task<List<CompanyAreaResponse?>> GetCompanyCoverageAreaById(int Id);

        Task<PaginationResponse<ShowTechnicianServiceResponse,FilterTechnicianResponse>> GetAllTechnicianProfiles(FilterTechnicianRequest filter, int page);

        Task<ShowTechnicianServiceResponse?> GetTechnicianProfileById(int id);
        
        Task<bool> ApproveCompanyCreate(int notid);


        Task<bool> ApproveCompanyUpdate(int notid);

        Task<bool> RejectCompanyCreate(int notificationId);


        Task<bool> RejectCompanyUpdate(int notificationId);

    }
}


