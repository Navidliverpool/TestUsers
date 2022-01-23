using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestUsers.Models;

namespace TestUsers.ViewModels
{
    public class Home2VM
    {
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> AllCategories { get; set; }
        private IEnumerable<int> _selectedCategories;
        public IEnumerable<int> SelectedCategories
        {
            get
            {
                if (_selectedCategories == null)
                {
                    _selectedCategories = Category.Brands.Select(m => m.BrandId).ToList();
                }
                return _selectedCategories;
            }
            set { _selectedCategories = value; }
        }
    }
}