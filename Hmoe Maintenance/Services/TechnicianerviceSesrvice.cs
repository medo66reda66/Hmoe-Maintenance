using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class TechnicianerviceSesrvice : ITechnicianerviceSesrvice
    {
        private readonly AppDBcontext _Context;

        public TechnicianerviceSesrvice(AppDBcontext context)
        {
            _Context = context;
        }

        public async Task<PaginationResponse<ShowTechnicianServiceResponse>> GetAllTechnicianService(FilterTechnicianServiceRequest filter,int page)
        {
            var GetAll =  _Context.TechnicianServices
                .Include(e=>e.TechnicianProfile)
                .ThenInclude(w => w.Company)
                .Include(e=>e.ServiceCategory)
                .AsQueryable();

            var showGetAll = GetAll.Select(e => new ShowTechnicianServiceResponse
            {
                id=e.Id,
                Fullnametechnicia= e.TechnicianProfile.Fullname,
                NationalIdtec = e.TechnicianProfile.NationalId,
                Emailtec = e.TechnicianProfile.Email,
                CreatedAt = DateTime.UtcNow,
                CompanyName = e.TechnicianProfile.Company.Name,
                DescriptionCompany = e.TechnicianProfile.Company.Description,
                EmailCompany = e.TechnicianProfile.Company.Email,
                Descriptionservicecategory = e.ServiceCategory.Description,
                tecnicalservice = e.ServiceCategory.Name,
            });

            FilterTechnicianServiceResponse filterResponse = new();

            if (filter.FullName != null)
            {
                showGetAll = showGetAll
                    .Where(e => e.Fullnametechnicia.Contains(filter.FullName));

                filterResponse.FullName = filter.FullName;
            }
            if (filter.Email != null)
            {
                showGetAll = showGetAll
                    .Where(e => e.Emailtec.Contains(filter.Email));

                filterResponse.Email = filter.Email;
            }
            if (filter.CompanyName != null)
            {
                showGetAll = showGetAll
                    .Where(e => e.CompanyName.Contains(filter.CompanyName));

                filterResponse.CompanyName = filter.CompanyName;
            }
            if (filter.ServiceName != null)
            {
                showGetAll = showGetAll
                    .Where(e => e.tecnicalservice.Contains(filter.ServiceName));

                filterResponse.ServiceName = filter.ServiceName;
            }
            if (filter.NationalId != null)
            {
                showGetAll = showGetAll
                    .Where(e => e.NationalIdtec == filter.NationalId);

                filterResponse.NationalId = filter.NationalId;
            }

            var result =await PaginationService.PaginateAsync(showGetAll, page, 5);


            return result;
        }

        public async Task<ShowTechnicianServiceResponse?> GetTechnicianServiceById(int id)
        {
            var technicianService = await _Context.TechnicianServices
                .Include(e => e.TechnicianProfile)
                .ThenInclude(t => t.Company)
                .Include(e => e.ServiceCategory)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (technicianService == null)
                return null;

            return new ShowTechnicianServiceResponse
            {
                id = technicianService.Id,
                Fullnametechnicia = technicianService.TechnicianProfile.Fullname,
                NationalIdtec = technicianService.TechnicianProfile.NationalId,
                Emailtec = technicianService.TechnicianProfile.Email,
                CreatedAt = DateTime.UtcNow, // أو DateTime.UtcNow لو ده المطلوب عندك
                CompanyName = technicianService.TechnicianProfile.Company.Name,
                DescriptionCompany = technicianService.TechnicianProfile.Company.Description,
                EmailCompany = technicianService.TechnicianProfile.Company.Email,
                Descriptionservicecategory = technicianService.ServiceCategory.Description,
                tecnicalservice = technicianService.ServiceCategory.Name,
            };
        }

        public async Task<TechnicianService> createTechnicianervice(TechnicianServiceRequest technicianService,string tecid)
        {
            var technicianProfileId =await _Context.TechnicianProfiles.FirstOrDefaultAsync(e => e.UserId == tecid);
            if (technicianProfileId == null)
            {
                return null;
            }

            var cretechnic = new TechnicianService
            {
                ServiceCategoryId = technicianService.ServiceCategoryId,
                TechnicianProfileId = technicianProfileId.Id,
            };

           await _Context.TechnicianServices.AddAsync(cretechnic);
           await  _Context.SaveChangesAsync();

            return cretechnic;
        }
        public async Task<TechnicianService> UpdateTechnicianervice(int id , TechnicianServiceRequest technicianService)
        {
            var updtechnic = await _Context.TechnicianServices.FirstOrDefaultAsync(e=>e.Id == id);
            if (updtechnic == null) 
            {
                return null;
            }
            updtechnic.ServiceCategoryId = technicianService.ServiceCategoryId;

            _Context.TechnicianServices.Update(updtechnic);
             await  _Context.SaveChangesAsync();

            return updtechnic;
        }
        public async Task<bool> DeleteTechnicianervice(int id , TechnicianService technicianService)
        {
            var deltechnician = await _Context.TechnicianServices.FirstOrDefaultAsync (e=>e.Id == id);
            if (deltechnician == null)
            {
                return false;
            }
            _Context.TechnicianServices.Remove(deltechnician);
           await  _Context.SaveChangesAsync();

            return true;
        }
       
    }
}
