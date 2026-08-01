using Hmoe_Maintenance.Models;
using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class TechnicianServiceRequest
    {
        [Required]

        public int ServiceCategoryId { get; set; }
    }
}
