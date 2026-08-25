using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class CompanyCoverageAreaService : ICompanyCoverageAreaService
    {
        private readonly AppDBcontext _context;

        public CompanyCoverageAreaService(AppDBcontext context)
        {
            _context = context;
        }

        public async Task<List<CompanyAreaResponse>?> GetMyCompanyCoverageArea(string companyId)
        {
            var coverageArea =  _context.CompanyCoverageAreas
                .Include(e=>e.Company)
                .Include(e=>e.Company.applicationUser)
                .Where(c => c.Company.ApplicationUserId == companyId);

            var showCompanyArea =await coverageArea.Select(coverageArea => new CompanyAreaResponse
            {
                Id = coverageArea.Id,
                Governorate = coverageArea.Governorate,
                City = coverageArea.City,
                Area = coverageArea.Area,
                IsActive = coverageArea.IsActive,
                companyName = coverageArea.Company.Name,
                companyOwnerName = coverageArea.Company.applicationUser.FullName,
                DiscriptionCompany = coverageArea.Company.Description,
                phoneNumberCompany = coverageArea.Company.PhoneNumber,
                EmailCompany = coverageArea.Company.Email!,
                CommercialRegistrationNumber=coverageArea.Company.CommercialRegistrationNumber,
                LicenseImageUrl = coverageArea.Company.LicenseImageUrl,
                CommercialRegistrationImageUrl = coverageArea.Company.CommercialRegistrationNumber,
                logourl=coverageArea.Company.LogoUrl
            }).ToListAsync();

            return showCompanyArea;
        }
        public async Task<CompanyCoverageArea> CreateCompanyCoverageArea(CreateCompanyCoverageAreaRequest request, string userId)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
            if (company == null)
            {
                return null;
            }

            var coverageArea = new CompanyCoverageArea
            {
                CompanyId = company.Id,
                Governorate = request.Governorate,
                City = request.City,
                Area = request.Area,
                IsActive = request.IsActive
            };

            _context.CompanyCoverageAreas.Add(coverageArea);
            await _context.SaveChangesAsync();

            return coverageArea;
        }
        public async Task<CompanyCoverageArea> UpdateCompanyCoverageArea(int id, UpdateCompanyCoverageAreaRequest request)
        {
            var coverageArea = await _context.CompanyCoverageAreas.FirstOrDefaultAsync(c => c.Id == id);
            if (coverageArea == null)
            {
                return null;
            }
            coverageArea.Governorate = request.Governorate;
            coverageArea.City = request.City;
            coverageArea.Area = request.Area;
            coverageArea.IsActive = request.IsActive;

            _context.CompanyCoverageAreas.Update(coverageArea);
            await _context.SaveChangesAsync();
            return coverageArea;

        }
        public async Task<bool> DeleteCompanyCoverageArea(int id)
        {
            var coverageArea = await _context.CompanyCoverageAreas.FirstOrDefaultAsync(c => c.Id == id);
            if (coverageArea == null)
            {
                return false;
            }
            _context.CompanyCoverageAreas.Remove(coverageArea);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
