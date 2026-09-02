using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
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
        public async Task<PaginationResponse<ShowCompanyServiceResponse,FiltercompanyserviceResponse>> GetAllCompanyServices(FiltercompanyserviceResquest filtercompanyservice ,int page)
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

            var result = await PaginationService.PaginateAsync(showCompanyService, page, filtercompanyserviceResponse, 5);

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

        public async Task<ShowCompanyServiceResponse> CreateServiceTomyCompany(string compid,CreateCompanyServiceRequest createCompanyService)
        {
            var MyCompany =await _context.Companies.FirstOrDefaultAsync(e=>e.ApplicationUserId == compid);
            if (MyCompany == null)
            {
                return null;
            }

            var companyService = new Models.CompanyService()
            {
                ServiceCategoryId = createCompanyService.ServiceCategoryId,
                CompanyId = MyCompany.Id,
                Description = createCompanyService.Description,
                InspectionPrice = createCompanyService.InspectionPrice,
                StartingPrice = createCompanyService.StartingPrice,
                IsActive = createCompanyService.IsActive
            };

            await _context.CompanyServices.AddAsync(companyService);
            await _context.SaveChangesAsync();

            var showCompanyService = new ShowCompanyServiceResponse()
            {
                companyName = MyCompany.Name,
                companyDescription = MyCompany.Description,
                ServiceId = companyService.ServiceCategoryId,
                InspectionPrice = companyService.InspectionPrice,
                StartingPrice = companyService.StartingPrice,
                IsActive = companyService.IsActive,
                ServiceDescription = companyService.Description
            };

            return showCompanyService;
        }
       
    }
}
