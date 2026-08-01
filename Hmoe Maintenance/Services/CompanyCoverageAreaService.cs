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

        public async Task<List<CompanyAreaResponse>> GetAllCompanyCoverageAreas()
        {
            var coverageAreas = await _context.CompanyCoverageAreas.Include(c => c.Company).ToListAsync();
            var showCompanyAreas = coverageAreas.Select(c => new CompanyAreaResponse
            {
                Id = c.Id,
                Governorate = c.Governorate,
                City = c.City,
                Area = c.Area,
                IsActive = c.IsActive,
                companyName = c.Company.Name,
                DiscriptionCompany = c.Company.Description,
                phoneNumberCompany = c.Company.PhoneNumber,
                EmailCompany = c.Company.Email!
            }).ToList();

            return showCompanyAreas;
        }
        public async Task<CompanyAreaResponse?> GetCompanyCoverageAreaById(int id)
        {
            var coverageArea = await _context.CompanyCoverageAreas.Include(e=>e.Company).FirstOrDefaultAsync(c => c.Id == id);
            var showCompanyArea = new CompanyAreaResponse
            {
                Id = coverageArea.Id,
                Governorate = coverageArea.Governorate,
                City = coverageArea.City,
                Area = coverageArea.Area,
                IsActive = coverageArea.IsActive,
                companyName = coverageArea.Company.Name,
                DiscriptionCompany = coverageArea.Company.Description,
                phoneNumberCompany = coverageArea.Company.PhoneNumber,
                EmailCompany = coverageArea.Company.Email!
            };
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
