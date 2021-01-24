using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models
{
    public static class EntityPaginationExtentions
    {
        public static async Task<Pagination<T>> ToEntityPaginated<T>(this IQueryable<T> query, PaginationEntry pagination)
        {
            int count = query.Count();
            int totalPages = (int)Math.Ceiling(count / (double)pagination.PerPage);
            string endPoint = typeof(T).Name.ToLower();
            return new Pagination<T>()
            {
                Total = count,
                Pages = totalPages,
                Page = pagination.Page,
                PerPage = pagination.PerPage,
                Result = await query.Skip((pagination.Page - 1) * pagination.PerPage).Take(pagination.PerPage).ToListAsync(),
                Previous = (pagination.Page > 1) ?
                $"{endPoint}?size={pagination.PerPage}&page={pagination.Page - 1}" : "",
                Next = (pagination.Page < totalPages) ?
                $"{endPoint}?size={pagination.PerPage}&page={pagination.Page + 1}" : ""
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
