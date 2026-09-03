using Hmoe_Maintenance.DTOs.Response;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IProfileUserService
    {
        Task<ProfileUserResponse> GetProfileUserByIdAsync(string userId);
        Task<bool> UpdateProfileUserAsync(ProfileUserResponse profileUser);

        Task<bool> UpdatePasswordAsync(ProfileUserResponse profileUser);
    }
}
