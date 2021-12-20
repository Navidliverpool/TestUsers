using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TestUsers.Models
{
    public class DealerVM
    {
        public Dealer Dealer { get; set; }
        public IEnumerable<SelectListItem> AllBrands { get; set; }
        private List<int> _selectedBrands;
        public List<int> SelectedBrands
        {
            get
            {
                if (_selectedBrands == null)
                {
                    _selectedBrands = Dealer.Brands.Select(m => m.BrandId).ToList();
                }
                return _selectedBrands;
            }
            set { _selectedBrands = value; }
        }
    }
}