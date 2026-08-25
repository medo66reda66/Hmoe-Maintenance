using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class TechnicianProfileServices : ITechnicianProfileServices
    {
        private readonly AppDBcontext _Context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TechnicianProfileServices(AppDBcontext context, UserManager<ApplicationUser> userManager)
        {
            _Context = context;
            _userManager = userManager;
        }

        public async Task<TechnincianProfileResponse?> GetMyTechnicianProfile(string techid)
        {
            var profile = await _Context.TechnicianProfiles
                .Include(t => t.Company)
                .Include(t => t.User)
                .Include(t => t.TechnicianServices)
                .FirstOrDefaultAsync(t => t.UserId == techid);

            if (profile == null)
            {
                return null;
            }

            var showProfile = new TechnincianProfileResponse
            {
                Id = profile.Id,
                CompanyName = profile.Company != null ? profile.Company.Name : string.Empty,
                FullName = profile.Fullname,
                PhoneNumber = profile.User != null ? profile.User.PhoneNumber! : string.Empty,
                Email = profile.User != null ? profile.User.Email! : string.Empty,
                NationalId = profile.NationalId,
                ProfileImageUrl = profile.ProfileImageUrl,
                NationalIdFrontImageUrl = profile.NationalIdFrontImageUrl,
                NationalIdBackImageUrl = profile.NationalIdBackImageUrl,
                TechnicianDocumentUrl = profile.TechnicianDocumentUrl,
                technicianServices = profile.TechnicianServices.Select(e => e.ServiceCategory)!,
                YearsOfExperience = profile.YearsOfExperience,
                Status = profile.Status,
                ApprovedByUserId = profile.ApprovedByUserId,
                RevenueShare = profile.RevenueShare,
                Bio = profile.Bio,
                AverageRating = profile.AverageRating,
                TotalCompletedJobs = profile.TotalCompletedJobs,
                IsAvailable = profile.IsAvailable,
                IsActive = profile.IsActive,
                CreatedAt = profile.CreatedAt
            };

            return showProfile;
        }

        public async Task<TechnicianProfile> CreateTechniciaProfile(CreateTechnicianProfileRequest request , string userId)
        {
            var tech = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var technicianProfile = new TechnicianProfile
            {
                UserId= userId,
                CompanyId = request.CompanyId,
                NationalId = request.NationalId,
                Fullname = request.Fullname,
                YearsOfExperience = request.YearsOfExperience,
                Email= tech.Email,
                PhoneNumper= request.PhoneNumper,
                Bio = request.Bio,
                Status = TechnicianStatus.Pending,
                IsActive = true,
                IsAvailable = false,
                CreatedAt = DateTime.UtcNow
            };
            if(request.NationalIdFrontImageUrl !=null)
            {
                var FilenameFFFrontImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(request.NationalIdFrontImageUrl.FileName);
                var filePathFrontImageUrl = Path.Combine("wwwroot", "FrontImageUrl", FilenameFFFrontImageUrl);
                using (var stream = new FileStream(filePathFrontImageUrl, FileMode.Create))
                {
                    await request.NationalIdFrontImageUrl.CopyToAsync(stream);
                }
                technicianProfile.NationalIdFrontImageUrl = FilenameFFFrontImageUrl;
            }
            if(request.NationalIdBackImageUrl !=null)
            {
                var FilenameFFBackImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(request.NationalIdBackImageUrl.FileName);
                var filePathBackImageUrl = Path.Combine("wwwroot", "BackImageUrl", FilenameFFBackImageUrl);
                using (var stream = new FileStream(filePathBackImageUrl, FileMode.Create))
                {
                    await request.NationalIdBackImageUrl.CopyToAsync(stream);
                }
                technicianProfile.NationalIdBackImageUrl = FilenameFFBackImageUrl;
            }
            if(request.ProfileImageUrl !=null)
            {
                var FilenameFFProfileImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(request.ProfileImageUrl.FileName);
                var filePathProfileImageUrl = Path.Combine("wwwroot", "ProfileTechnicianImageUrl", FilenameFFProfileImageUrl);
                using (var stream = new FileStream(filePathProfileImageUrl, FileMode.Create))
                {
                    await request.ProfileImageUrl.CopyToAsync(stream);
                }
                technicianProfile.ProfileImageUrl = FilenameFFProfileImageUrl;
            }
            if(request.TechnicianDocumentUrl !=null)
            {
                var FilenameTechnicianDocumentUrl = Guid.NewGuid().ToString() + Path.GetExtension(request.TechnicianDocumentUrl.FileName);
                var filePathTechnicianDocumentUrl = Path.Combine("wwwroot", "TechnicianDocumentUrl", FilenameTechnicianDocumentUrl);
                using (var stream = new FileStream(filePathTechnicianDocumentUrl, FileMode.Create))
                {
                    await request.TechnicianDocumentUrl.CopyToAsync(stream);
                }
                technicianProfile.TechnicianDocumentUrl = FilenameTechnicianDocumentUrl;
            }

            _Context.TechnicianProfiles.Add(technicianProfile);
            await _Context.SaveChangesAsync();

            var ownerCompany = await _Context.Companies.FirstOrDefaultAsync(x => x.Id == technicianProfile.CompanyId);
            var notification = new Notification
            {
                UserId = ownerCompany.ApplicationUserId!,
                Title = "New Technician Application",
                Message = $"{request.Fullname} has applied to join your company. Please review the application and approve or reject the request.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationType.TechnicianApplicationReceived,
                RelatedEntityId = userId
            };
               _Context.Notification.Add(notification);
               await _Context.SaveChangesAsync();

            return technicianProfile ;
        }

        public async Task<TechnicianProfile?> UpdateTechniciaProfile(int id, UpdateTechniciaProfileRequest request)
        {
            
            var profile = await _Context.TechnicianProfiles.FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null && profile.Status == TechnicianStatus.Pending)
            {
                return null;
            }
            var copuprofile = await _Context.TechnicianProfileCopies.FirstOrDefaultAsync(p => p.UserId == profile.UserId && p.IsAvailable == true );
            if( copuprofile == null )
            {
                return null;
            }

            profile.CompanyId = request.CompanyId;
            profile.Fullname = request.Fullname;
            profile.NationalId = request.NationalId;
            profile.YearsOfExperience = request.YearsOfExperience;
            profile.Bio = request.Bio;
            profile.IsAvailable = request.IsAvailable;
            profile.IsActive = request.IsActive;
            profile.Email = request.Email;
            profile.PhoneNumper = request.PhoneNumper;
            profile.IsActive = false;

            copuprofile.IsAvailable = false;

            if (request.ProfileImageUrl != null)
            {
                if (!string.IsNullOrEmpty(profile.ProfileImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileTechnicianImageUrl", profile.ProfileImageUrl);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                var newFilename = Guid.NewGuid().ToString() + Path.GetExtension(request.ProfileImageUrl.FileName);
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileTechnicianImageUrl", newFilename);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await request.ProfileImageUrl.CopyToAsync(stream);
                }
                profile.ProfileImageUrl = newFilename;
            }
            else
            {
                profile.ProfileImageUrl = profile.ProfileImageUrl;
            }

            if (request.NationalIdFrontImageUrl != null)
            {
                if (!string.IsNullOrEmpty(profile.NationalIdFrontImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FrontImageUrl", profile.NationalIdFrontImageUrl);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                var newFilename = Guid.NewGuid().ToString() + Path.GetExtension(request.NationalIdFrontImageUrl.FileName);
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FrontImageUrl", newFilename);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await request.NationalIdFrontImageUrl.CopyToAsync(stream);
                }
                profile.NationalIdFrontImageUrl = newFilename;
            }
            else
            {
                profile.NationalIdFrontImageUrl = profile.NationalIdFrontImageUrl; 
            }

            if (request.NationalIdBackImageUrl != null)
            {
                if (!string.IsNullOrEmpty(profile.NationalIdBackImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackImageUrl", profile.NationalIdBackImageUrl);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                var newFilename = Guid.NewGuid().ToString() + Path.GetExtension(request.NationalIdBackImageUrl.FileName);
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackImageUrl", newFilename);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await request.NationalIdBackImageUrl.CopyToAsync(stream);
                }
                profile.NationalIdBackImageUrl = newFilename;
            }
            else
            {
                profile.NationalIdBackImageUrl= profile.NationalIdBackImageUrl;
            }

            if (request.TechnicianDocumentUrl != null)
            {
                if (!string.IsNullOrEmpty(profile.TechnicianDocumentUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TechnicianDocumentUrl", profile.TechnicianDocumentUrl);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                var newFilename = Guid.NewGuid().ToString() + Path.GetExtension(request.TechnicianDocumentUrl.FileName);
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TechnicianDocumentUrl", newFilename);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await request.TechnicianDocumentUrl.CopyToAsync(stream);
                }
                profile.TechnicianDocumentUrl = newFilename;
            }
            else
            {
                profile.TechnicianDocumentUrl= profile.TechnicianDocumentUrl;
            }

            if (request.TechnicianDocumentUrl != null || request.TechnicianDocumentUrl != null || request.NationalIdFrontImageUrl !=null || request.NationalIdBackImageUrl !=null)
            {
                var ownerCompany = await _Context.Companies.FirstOrDefaultAsync(x => x.Id == profile.CompanyId);
                var notification = new Notification
                {
                    UserId = ownerCompany.ApplicationUserId!,
                    Title = "Technician Profile Update Request",
                    Message = $"{request.Fullname} has requested to update their profile information. Please review the changes and approve or reject the request.",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    Type = NotificationType.TechnicianApplicationupdate,
                    RelatedEntityId = profile.UserId
                };
                _Context.Notification.Add(notification);
                await _Context.SaveChangesAsync();
            }

            ////////////////
            //////
            ///Admin
            //////
            ///
                _Context.TechnicianProfiles.Update(profile);
                await _Context.SaveChangesAsync();
          
            return profile;
        }
        public async Task<bool> DeleteTechnicianProfile(int id)
        {
            var profile = await _Context.TechnicianProfiles.FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null)
            {
                return false;
            }

            // Delete Profile Image
            if (!string.IsNullOrEmpty(profile.ProfileImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileTechnicianImageUrl", profile.ProfileImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Delete National ID Front Image
            if (!string.IsNullOrEmpty(profile.NationalIdFrontImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FrontImageUrl", profile.NationalIdFrontImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Delete National ID Back Image
            if (!string.IsNullOrEmpty(profile.NationalIdBackImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackImageUrl", profile.NationalIdBackImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Delete Technician Document
            if (!string.IsNullOrEmpty(profile.TechnicianDocumentUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TechnicianDocumentUrl", profile.TechnicianDocumentUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _Context.TechnicianProfiles.Remove(profile);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
