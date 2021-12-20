using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TestUsers.Models
{
    public class BrandVM
    {
        public Brand Brand { get; set; }
        public IEnumerable<SelectListItem> AllDealers { get; set; }
        private List<int> _selectedDealers;
        public List<int> SelectedDealers
        {
            get
            {
                if (_selectedDealers == null)
                {
                    _selectedDealers = Brand.Dealers.Select(m => m.DealerId).ToList();
                }
                return _selectedDealers;
            }
            set { _selectedDealers = value; }
        }

        public IEnumerable<SelectListItem> AllMotorcycles { get; set; }
        private List<int> _selectedMotorcycles;
        public List<int> SelectedMotorcycles
        {
            get
            {
                if (_selectedMotorcycles == null)
                {
                    _selectedMotorcycles = Brand.Motorcycles.Select(m => m.MotorcycleId).ToList();
                }
                return _selectedMotorcycles;
            }
            set { _selectedMotorcycles = value; }
        }
    }
}