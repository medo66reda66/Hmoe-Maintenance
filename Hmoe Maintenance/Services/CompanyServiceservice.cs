using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
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

        public async Task<IEnumerable<CompanyServiceResponse>> GetAllCompanyServices()
        {
            var companyServices = await _context.CompanyServices.Include(e => e.Company).Include(e => e.ServiceCategory).AsQueryable().ToListAsync();
           var showCompanyServices = companyServices.Select(e => new CompanyServiceResponse
            {
                Id = e.Id,
                CompanyName = e.Company.Name,
                ServiceCategoryName = e.ServiceCategory.Name,
                InspectionPrice = e.InspectionPrice,
                StartingPrice = e.StartingPrice,
                Description = e.Description,
                IsActive = e.IsActive
            });
            return showCompanyServices;
        }

        public async Task<CompanyServiceResponse> GetCompanyServiceById(int id)
        {
            var companyService = await _context.CompanyServices.Include(e => e.Company).Include(e => e.ServiceCategory).FirstOrDefaultAsync(e => e.Id == id);
            var showCompanyService =  new CompanyServiceResponse
            {
                Id = companyService.Id,
                CompanyName = companyService.Company.Name,
                ServiceCategoryName = companyService.ServiceCategory.Name,
                InspectionPrice = companyService.InspectionPrice,
                StartingPrice = companyService.StartingPrice,
                Description = companyService.Description,
                IsActive = companyService.IsActive
            };
            return showCompanyService;
        }

        public async Task<Models.CompanyService?> CreateCompanyService(CreateCompanyServiceRequest request, string applicationUserId)
        {
            var companyExists = await _context.Companies.FirstOrDefaultAsync(c => c.ApplicationUserId == applicationUserId && c.IsActive);
            if (companyExists == null)
            {
                return null;
            }
            var companyService = new Models.CompanyService
            {
                CompanyId = companyExists.Id,
                ServiceCategoryId = request.ServiceCategoryId,
                InspectionPrice = request.InspectionPrice,
                StartingPrice = request.StartingPrice,
                Description = request.Description,
                IsActive = request.IsActive
            };

            _context.CompanyServices.Add(companyService);
            await _context.SaveChangesAsync();

            return companyService;
        }
        public async Task<Models.CompanyService?> UpdateCompanyService(int id, UpdateCompanyServiceRequest request)
        {
            var companyService = await _context.CompanyServices.FindAsync(id);
            if (companyService == null)
            {
                return null;
            }
            companyService.ServiceCategoryId = request.ServiceCategoryId;
            companyService.InspectionPrice = request.InspectionPrice;
            companyService.StartingPrice = request.StartingPrice;
            companyService.Description = request.Description;
            companyService.IsActive = request.IsActive;

            await _context.SaveChangesAsync();
            return companyService;
        }

        public async Task<bool> DeleteCompanyService(int id)
        {
            var companyService = await _context.CompanyServices.FindAsync(id);
            if (companyService == null)
            {
                return false;
            }
            _context.CompanyServices.Remove(companyService);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
