using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItineriesApi.Models.DAL
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext context;
        public EFCategoryRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Category> GetCategories => context.Categories;
    }
}
