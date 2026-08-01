using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
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

        public async Task<List<ShowTechnicianServiceResponse>> GetAllTechnicianService()
        {
            var GetAll = await _Context.TechnicianServices
                .Include(e=>e.TechnicianProfile)
                .ThenInclude(w=>w.Company)
                .Include(e=>e.ServiceCategory).AsQueryable().ToListAsync();

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
                Nameservicecategory = e.ServiceCategory.Name,
            }).ToList();
            return showGetAll;
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
                Nameservicecategory = technicianService.ServiceCategory.Name,
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
