using Hmoe_Maintenance.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class PaginationService
    {
        public static async Task<PaginationResponse<T>> PaginateAsync<T>(
        IQueryable<T> query,
        int page ,
        int pageSize = 5)
        {
            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize
            );

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<T>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Data = data
            };
        }
    }
}
