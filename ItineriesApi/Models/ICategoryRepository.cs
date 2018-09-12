using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItineriesApi.Models
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetCategories { get; }
    }
}
