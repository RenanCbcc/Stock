using System.Linq;

namespace Stock_Back_End.Models.CategoryModels
{
    public static class CategoryFilterExtentions
    {
        public static IQueryable<Category> AplyFilter(this IQueryable<Category> query, CategoryFilter filter)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Title))
                {
                    query = query.Where(c => c.Title.Contains(filter.Title));
                }

            }
            return query;
        }

    }
    public class CategoryFilter
    {
        public string Title { get; set; }
    }
}
