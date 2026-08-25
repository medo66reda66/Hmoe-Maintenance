using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Hmoe_Maintenance.Services
{
    public class CompanyProfileAndDetailsService : ICompanyProfileAndDetailsService
    {
        private readonly AppDBcontext _context;

        public CompanyProfileAndDetailsService(AppDBcontext context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<ServiceCategory>> GetAllServiceCategories(FilterServiceCategoryRequest filter,int page)
        {
            var serviceCategories =  _context.ServiceCategories
                .AsNoTracking();

            FilterServiceCategoryResponse filterResponse = new();

            if (filter.Name != null)
            {
                serviceCategories = serviceCategories
                    .Where(e => e.Name.Contains(filter.Name));

                filterResponse.Name = filter.Name;
            }

            if (filter.IsActive.HasValue)
            {
                serviceCategories = serviceCategories
                    .Where(e => e.IsActive == filter.IsActive.Value);

                filterResponse.IsActive = filter.IsActive.Value;
            }

            var result =await PaginationService.PaginateAsync(serviceCategories, page, 10);

            return result;
        }
        public async Task<CompanyProfileResponse> GetCompanyProfileAndDetailsServiceById(int companyId)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(P=>P.Id == companyId);

            var techs = _context.TechnicianProfileCopies
                .Include(s=>s.User)
                .AsQueryable()
                .AsNoTracking()
                .Where(t => t.CompanyId == companyId);

            var camparea = _context.CompanyCoverageAreas
                .AsNoTracking()
                .AsQueryable()
                .Where(c => c.CompanyId == companyId);

            var services = _context.CompanyServices
                .Include(s => s.ServiceCategory)
                .AsNoTracking()
                .AsQueryable()
                .Where(c => c.CompanyId == companyId);

            var showCompanyProfileAndDetailsService = new CompanyProfileResponse
            {
                Name = company.Name,
                Description = company.Description,
                LogoUrl = company.LogoUrl,
                PhoneNumber = company.PhoneNumber,
                Email = company.Email,
                AverageRating = company.AverageRating,
                TotalReviews = company.TotalReviews,
                TechnicianCount = await techs.CountAsync(),
                CompletedRequestsCount = company.CompletedRequestsCount,
                Technicians = await techs.Select(t => new TechnicianincompanyProfileResponse
                {
                    Id = t.Id,
                    Fullname = t.Fullname,
                    Email = t.User.Email,
                    PhoneNumber = t.PhoneNumper,
                    AverageRating = t.AverageRating,
                    revenueShare = t.RevenueShare,
                    TotalCompletedJobs = t.TotalCompletedJobs,
                    Bio = t.Bio,
                    YearsOfExperience = t.YearsOfExperience,
                    IsActive= t.IsActive,
                    IsAvailable = t.IsAvailable,
                }).ToListAsync(),
                CoverageAreas = await camparea.Select(c => new CompanyCoverageAreaProfileResponse
                {
                    Id = c.Id,
                    Governorate = c.Governorate,
                    City = c.City,
                    Area = c.Area,
                    IsActive = c.IsActive
                }).ToListAsync(),
            };

            return showCompanyProfileAndDetailsService;
        }
        public async Task<PaginationResponse<CompanyProfileResponse>> AllCompanyProfileAndDetailsService(int serviceid,FilterCompanyProfileRequest filter,int page)
        {
            var company =  _context.CompanyServices.Where(s => s.ServiceCategoryId == serviceid)
                .Include(e=>e.Company)
              .AsNoTracking();

            var showCompanyProfileAndDetailsService = company.Select(company=> new CompanyProfileResponse
            {
                Name = company.Company.Name,
                Description = company.Company.Description,
                LogoUrl = company.Company.LogoUrl,
                PhoneNumber = company.Company.PhoneNumber,
                Email = company.Company.Email,
                AverageRating = company.Company.AverageRating,
                TotalReviews = company.Company.TotalReviews,
                CompletedRequestsCount = company.Company.CompletedRequestsCount,
                IsActive = company.Company.IsActive,
                CoverageAreas = company.Company.CompanyCoverageAreas.Select(c => new CompanyCoverageAreaProfileResponse
                {
                    Id = c.Id,
                    Governorate = c.Governorate,
                    City = c.City,
                    Area = c.Area,
                    IsActive = c.IsActive
                })
               .ToList()
            });

            FilterCompanyProfileResponse filterResponse = new();

            if (filter.Name != null)
            {
                showCompanyProfileAndDetailsService = showCompanyProfileAndDetailsService.Where(e =>
                    e.Name.Contains(filter.Name));

                filterResponse.Name = filter.Name;
            }

            if (filter.Governorate != null)
            {
                showCompanyProfileAndDetailsService = showCompanyProfileAndDetailsService.Where(e =>
                    e.CoverageAreas.Any(c =>
                        c.Governorate.Contains(filter.Governorate)));

                filterResponse.Governorate = filter.Governorate;
            }

            if (filter.City != null)
            {
                showCompanyProfileAndDetailsService = showCompanyProfileAndDetailsService.Where(e =>
                    e.CoverageAreas.Any(c =>
                        c.City.Contains(filter.City)));

                filterResponse.City = filter.City;
            }

            if (filter.IsActive.HasValue)
            {
                showCompanyProfileAndDetailsService = showCompanyProfileAndDetailsService.Where(c =>
                        c.IsActive == filter.IsActive.Value);

                filterResponse.IsActive = filter.IsActive.Value;
            }

            var result =await PaginationService.PaginateAsync(showCompanyProfileAndDetailsService, page, 10);
            return result;
        }

        
    }
}
