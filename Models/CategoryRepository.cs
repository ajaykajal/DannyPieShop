using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DannyPieShop.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext appDbContext;

        /// <summary>
        /// constructor which is using for initiliazer object of the category class using depndency injection
        /// </summary>
        public CategoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Getting all the category list form database
        /// </summary>
        public IEnumerable<Category> AllCategories => appDbContext.Categories;
    }
}
