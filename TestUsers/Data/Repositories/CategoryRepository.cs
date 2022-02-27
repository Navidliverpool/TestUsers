using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestUsers.Models;

namespace TestUsers.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        NavEcommerceDBfirstEntities19 _storeDB;
        public CategoryRepository(NavEcommerceDBfirstEntities19 storeDB)
        {
            _storeDB = storeDB;
        }
        public IQueryable<Category> GetCategories()
        {
            return _storeDB.Categories;
        }
    }
}