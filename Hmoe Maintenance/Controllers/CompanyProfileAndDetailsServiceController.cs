using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hmoe_Maintenance.DTOs.Request.filter;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfileAndDetailsServiceController : ControllerBase
    {
        private readonly ICompanyProfileAndDetailsService _companyProfileAndDetailsService;
        public CompanyProfileAndDetailsServiceController(ICompanyProfileAndDetailsService companyProfileAndDetailsService)
        {
            _companyProfileAndDetailsService = companyProfileAndDetailsService;
        }


        [HttpGet("GetAllServiceCategories")]
        public async Task<IActionResult> GetAllServiceCategories([FromQuery] FilterServiceCategoryRequest filter, [FromQuery] int page = 1)
        {
            var serviceCategories = await _companyProfileAndDetailsService.GetAllServiceCategories(filter, page);
            return Ok(serviceCategories);
        }

        [HttpGet("GetCompanyProfileAndDetailsServiceById/{companyId}")]
        public async Task<IActionResult> GetCompanyProfileAndDetailsServiceById(int companyId)
        {
            var companyProfile = await _companyProfileAndDetailsService.GetCompanyProfileAndDetailsServiceById(companyId);
            if (companyProfile == null)
            {
                return NotFound();
            }
            return Ok(companyProfile);
        }
       
        [HttpGet("GetAllCompanyProfileAndDetailsService/{serviceid}")]
        public async Task<IActionResult> GetAllCompanyProfileAndDetailsService(int serviceid, [FromQuery] FilterCompanyProfileRequest filter, [FromQuery] int page = 1)
        {
            var companyProfiles = await _companyProfileAndDetailsService.AllCompanyProfileAndDetailsService(serviceid, filter, page);
            if(companyProfiles.Datarequest == null || !companyProfiles.Datarequest.Any())
            {
                return NotFound();
            }
            return Ok(companyProfiles);
        }
        [HttpGet("GetTopTenCompany")]
        public async Task<IActionResult> GetTopTenCompany()
        {
            var result =await _companyProfileAndDetailsService.Gettoptencompany();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
