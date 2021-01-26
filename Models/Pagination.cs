using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models
{
    public static class EntityPaginationExtentions
    {
        public static async Task<Pagination<T>> ToEntityPaginated<T>(this IQueryable<T> query, PagingParams pagination)
        {
            int count = query.Count();
            int totalPages = (int)Math.Ceiling(count / (double)pagination.PageSize);
            string endPoint = typeof(T).Name.ToLower();
            return new Pagination<T>()
            {
                Total = count,
                Pages = totalPages,
                Page = pagination.PageNo,
                PerPage = pagination.PageSize,
                Result = await query.Skip((pagination.PageNo - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(),
                Previous = (pagination.PageNo > 1) ?
                $"{endPoint}?size={pagination.PageSize}&page={pagination.PageNo - 1}" : "",
                Next = (pagination.PageNo < totalPages) ?
                $"{endPoint}?size={pagination.PageSize}&page={pagination.PageNo + 1}" : ""
            };

        }
    }
    public class Pagination<T>
    {

        public int Total { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public int Page { get; set; }
        public List<T> Result { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
    }
}
