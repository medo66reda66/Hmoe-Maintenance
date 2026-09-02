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

        public async Task<PaginationResponse<ServiceCategory,FilterServiceCategoryResponse>> GetAllServiceCategories(FilterServiceCategoryRequest filter,int page)
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

            var result =await PaginationService.PaginateAsync(serviceCategories, page, filterResponse, 10);

            return result;
        }
        public async Task<CompanyProfileResponse> GetCompanyProfileAndDetailsServiceById(int companyCopyId)
        {
            var companyCopy = await _context.companyCopies
                .FirstOrDefaultAsync(P=>P.Id == companyCopyId);
            if(companyCopy == null)
            {
                return null;
            }

            var company =await _context.Companies.FirstOrDefaultAsync(e => e.ApplicationUserId == companyCopy.ApplicationUserId);

            var techs = _context.TechnicianProfileCopies
                .Include(s=>s.User)
                .AsQueryable()
                .AsNoTracking()
                .Where(t => t.CompanyCopyId == companyCopyId);
            if (!techs.Any())
            {
                return null;
            }

            var comparea = _context.CompanyCoverageAreas
                .AsNoTracking()
                .AsQueryable()
                .Where(c => c.CompanyId == company.Id);
            if (!comparea.Any())
            {
                return null;
            }

            var services = _context.CompanyServices
                .Include(s => s.ServiceCategory)
                .AsNoTracking()
                .AsQueryable()
                .Where(c => c.CompanyId == company.Id);
            if (!services.Any())
            {
                return null;
            }

            var showCompanyProfileAndDetailsService = new CompanyProfileResponse
            {
                Name = companyCopy.Name,
                Description = companyCopy.Description,
                LogoUrl = companyCopy.LogoUrl,
                PhoneNumber = companyCopy.PhoneNumber,
                Email = companyCopy.Email,
                AverageRating = companyCopy.AverageRating,
                TechnicianCount = await techs.CountAsync(),
                CompletedRequestsCount = companyCopy.CompletedRequestsCount,
                Technicians = await techs.Select(t => new TechnicianincompanyProfileResponse
                {
                    Id = t.Id,
                    Fullname = t.Fullname,
                    Email = t.User.Email,
                    PhoneNumber = t.PhoneNumper,
                    AverageRating = t.AverageRating,
                    TotalCompletedJobs = t.TotalCompletedJobs,
                    Bio = t.Bio,
                    YearsOfExperience = t.YearsOfExperience,
                    IsActive = t.IsActive,
                }).ToListAsync(),
                CoverageAreas = await comparea.Select(c => new CompanyCoverageAreaProfileResponse
                {
                    Id = c.Id,
                    Governorate = c.Governorate,
                    City = c.City,
                    Area = c.Area,
                    IsActive = c.IsActive
                }).ToListAsync(),
                companyServices =await services.Select(e => new CompanyServiceResponse
                {
                    ServiceCategoryName = e.ServiceCategory.Name,
                    InspectionPrice = e.InspectionPrice,
                    StartingPrice = e.StartingPrice,
                    IsActive = e.IsActive,
                    Description = e.Description,
                }).ToListAsync()
            };

            return showCompanyProfileAndDetailsService;
        }
        public async Task<PaginationResponse<CompanyProfileResponse,FilterCompanyProfileResponse>> AllCompanyProfileAndDetailsService(int serviceid,FilterCompanyProfileRequest filter,int page)
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
                IsActive = company.Company.IsActive
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

            var result =await PaginationService.PaginateAsync(showCompanyProfileAndDetailsService, page, filterResponse, 10);
            return result;
        }

        public async Task<List<ToptencompanyResponse>> Gettoptencompany()
        {
            var topCompany = await _context.companyCopies
                .OrderByDescending(e => e.AverageRating)
                .Skip(0)
                .Take(10)
                .Select(e => new ToptencompanyResponse
                {
                   Name = e.Name,
                   Avrgage =  e.AverageRating,
                   Logourl = e.LogoUrl
                })
                .ToListAsync();

            if(topCompany == null)
            {
                return null;
            }

            return topCompany;
        }
        
    }
}
