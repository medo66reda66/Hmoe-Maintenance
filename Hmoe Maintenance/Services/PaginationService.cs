using Hmoe_Maintenance.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class PaginationService
    {
        public static async Task<PaginationResponse<Trequest,Tresponse>> PaginateAsync<Trequest, Tresponse>(
        IQueryable<Trequest> queryRequest,
        int page,
        Tresponse? queryresponse = default,
        int pageSize = 5)
        {
            var totalCount = await queryRequest.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize
            );

            var datarequest = await queryRequest
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<Trequest, Tresponse>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Datarequest = datarequest,
                Dataresponse = queryresponse
            };
        }
    }
}
