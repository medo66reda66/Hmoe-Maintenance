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

        public async Task<PaginationResponse<ShowTechnicianServiceResponse,FilterTechnicianServiceResponse>> GetAllTechnicianService(FilterTechnicianServiceRequest filter,int page)
        {
            var GetAll =  _Context.TechnicianServices
                .Include(e=>e.TechnicianProfile)
                .ThenInclude(w => w.CompanyCopy)
                .Include(e=>e.ServiceCategory)
                .AsQueryable();

            var showGetAll = GetAll.Select(e => new ShowTechnicianServiceResponse
            {
                id=e.Id,
                Fullnametechnicia= e.TechnicianProfile.Fullname,
                NationalIdtec = e.TechnicianProfile.NationalId,
                Emailtec = e.TechnicianProfile.Email,
                CreatedAt = DateTime.UtcNow,
                CompanyName = e.TechnicianProfile.CompanyCopy.Name,
                DescriptionCompany = e.TechnicianProfile.CompanyCopy.Description,
                EmailCompany = e.TechnicianProfile.CompanyCopy.Email,
                servicecategoryname = e.ServiceCategory.Name,
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
                    .Where(e => e.servicecategoryname.Contains(filter.ServiceName));

                filterResponse.ServiceName = filter.ServiceName;
            }
            if (filter.NationalId != null)
            {
                showGetAll = showGetAll
                    .Where(e => e.NationalIdtec == filter.NationalId);

                filterResponse.NationalId = filter.NationalId;
            }

            var result =await PaginationService.PaginateAsync(showGetAll, page, filterResponse, 5);


            return result;
        }

        public async Task<ShowTechnicianServiceResponse?> GetTechnicianServiceById(int id)
        {
            var technicianService = await _Context.TechnicianServices
                .Include(e => e.TechnicianProfile)
                .ThenInclude(t => t.CompanyCopy)
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
                CompanyName = technicianService.TechnicianProfile.CompanyCopy.Name,
                DescriptionCompany = technicianService.TechnicianProfile.CompanyCopy.Description,
                EmailCompany = technicianService.TechnicianProfile.CompanyCopy.Email,
                Bio = technicianService.TechnicianProfile.Bio,
                ApprovedByUserId = technicianService.TechnicianProfile.ApprovedByUserId,
                PhoneNumper = technicianService.TechnicianProfile.PhoneNumper,
                IsActive = technicianService.TechnicianProfile.IsActive,
                IsAvailable = technicianService.TechnicianProfile.IsAvailable,
                servicecategoryname = technicianService.ServiceCategory.Name,
                servicecategorydescription= technicianService.ServiceCategory.Description
            };
        }

        public async Task<TechnicianService> createTechnicianervice(TechnicianServiceRequest technicianService,string tecid)
        {
            var technicianProfileId =await _Context.TechnicianProfiles.Include(e=>e.CompanyCopy).FirstOrDefaultAsync(e => e.UserId == tecid);
            if (technicianProfileId == null)
            {
                return null;
            }

            var serviceExists = await _Context.CompanyServices
                .Where(e => e.Company.ApplicationUserId == technicianProfileId.CompanyCopy.ApplicationUserId).ToListAsync();

            if (!serviceExists.Any())
            {
                return null;
            }
            var alreadyExists = await _Context.TechnicianServices
              .AnyAsync(e =>
            e.TechnicianProfileId == technicianProfileId.Id &&
            e.ServiceCategoryId == technicianService.ServiceCategoryId);

            if (alreadyExists)
            {
                return null;
            }

            var cretechnic = new TechnicianService
            {
                ServiceCategoryId = technicianService.ServiceCategoryId,
                TechnicianProfileId = technicianProfileId.Id,
            };

            foreach (var ser in serviceExists)
            {
                if (ser.ServiceCategoryId == technicianService.ServiceCategoryId)
                {
                    await _Context.TechnicianServices.AddAsync(cretechnic);
                    await _Context.SaveChangesAsync();
                }
            }
            return cretechnic;
        }
        public async Task<TechnicianService> UpdateTechnicianervice(int id , TechnicianServiceRequest technicianService)
        {
            var updtechnic = await _Context.TechnicianServices
                .Include(e=>e.TechnicianProfile).ThenInclude(e=>e.CompanyCopy).FirstOrDefaultAsync(e=>e.Id == id);
            if (updtechnic == null) 
            {
                return null;
            }
            var serviceExists = await _Context.CompanyServices.Where(e => e.Company.ApplicationUserId == updtechnic.TechnicianProfile.CompanyCopy.ApplicationUserId).ToListAsync();

            if (!serviceExists.Any())
            {
                return null;
            }
            updtechnic.ServiceCategoryId = technicianService.ServiceCategoryId;
            foreach (var ser in serviceExists)
            {
                if (ser.ServiceCategoryId == updtechnic.ServiceCategoryId)
                {
                    _Context.TechnicianServices.Update(updtechnic);
                    await _Context.SaveChangesAsync();
                }
            }
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
