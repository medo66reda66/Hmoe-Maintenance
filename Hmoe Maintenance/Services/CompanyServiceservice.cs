using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class CompanyServiceservice : ICompanyServiceservice
    {
        private readonly AppDBcontext _context;

        public CompanyServiceservice(AppDBcontext context)
        {
            _context = context;
        }

        //Admiiiiiin
        public async Task<PaginationResponse<ShowCompanyServiceResponse>> GetAllCompanyServices(FiltercompanyserviceResquest filtercompanyservice ,int page)
        {
            var companyService = _context.CompanyServices
                .Include(e => e.Company)
                .Include(e => e.ServiceCategory)
                .AsQueryable();

            var showCompanyService =  companyService.Select(companyService => new ShowCompanyServiceResponse
            {
                Id = companyService.Id,
                companyName = companyService.Company.Name,
                ServiceName = companyService.ServiceCategory.Name,
                InspectionPrice = companyService.InspectionPrice,
                StartingPrice = companyService.StartingPrice,
                companyDescription = companyService.Company.Description,
                ServiceDescription = companyService.ServiceCategory.Description,
                IconUrl = companyService.ServiceCategory.IconUrl,
                IsActive = companyService.IsActive
            });

            FiltercompanyserviceResponse filtercompanyserviceResponse = new FiltercompanyserviceResponse();
            if (filtercompanyservice.companyname != null)
            {
                showCompanyService = showCompanyService
                    .Where(e => e.companyName.Contains(filtercompanyservice.companyname));

                filtercompanyserviceResponse.CompanyName = filtercompanyservice.companyname;
            }

            if (filtercompanyservice.servicename != null)
            {
                showCompanyService = showCompanyService
                    .Where(e => e.ServiceName.Contains(filtercompanyservice.servicename));

                filtercompanyserviceResponse.ServiceName = filtercompanyservice.servicename;
            }

            if (filtercompanyservice.isActive.HasValue)
            {
                showCompanyService = showCompanyService
                    .Where(e => e.IsActive == filtercompanyservice.isActive.Value);

                filtercompanyserviceResponse.IsActive = filtercompanyservice.isActive.Value;
            }

            var result = await PaginationService.PaginateAsync(showCompanyService, page, 5);

            return result;
        }

        //company
        public async Task<ShowCompanyServiceResponse> GetoneCompanyServicesBYid(int id)
        {
            var companyService =await _context.CompanyServices
                .Include(e => e.Company)
                .Include(e => e.ServiceCategory)
                .FirstOrDefaultAsync(e=>e.Id == id);

            var showCompanyService =  new ShowCompanyServiceResponse
            {
                Id = companyService.Id,
                companyName = companyService.Company.Name,
                ServiceName = companyService.ServiceCategory.Name,
                InspectionPrice = companyService.InspectionPrice,
                StartingPrice = companyService.StartingPrice,
                companyDescription = companyService.Company.Description,
                ServiceDescription = companyService.ServiceCategory.Description,
                IconUrl = companyService.ServiceCategory.IconUrl,
                IsActive = companyService.IsActive
            };

            return showCompanyService;
        }

        /// company
        public async Task<List<CompanyServiceResponse>> GetMYCompanyServiceById(string compid)
        {
            var companyService = _context.CompanyServices
                .Include(e => e.Company)
                .Include(e => e.ServiceCategory).Where(e => e.Company.ApplicationUserId == compid);

            var showCompanyService =await companyService.Select(companyService => new CompanyServiceResponse
            {
                Id = companyService.Id,
                CompanyName = companyService.Company.Name,
                ServiceCategoryName = companyService.ServiceCategory.Name,
                InspectionPrice = companyService.InspectionPrice,
                StartingPrice = companyService.StartingPrice,
                Description = companyService.Description,
                IsActive = companyService.IsActive
            }).ToListAsync();

            return showCompanyService;
        }
       
    }
}
