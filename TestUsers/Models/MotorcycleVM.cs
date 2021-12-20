using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TestUsers.Models
{
    public class MotorcycleVM
    {
        public Motorcycle Motorcycle { get; set; }
        public IEnumerable<SelectListItem> AllDealers { get; set; }
        private IEnumerable<int> _selectedDealers;
        public IEnumerable<int> SelectedDealers
        {
            get
            {
                if (_selectedDealers == null)
                {
                    _selectedDealers = Motorcycle.Dealers.Select(m => m.DealerId).ToList();
                }
                return _selectedDealers;
            }
            set { _selectedDealers = value; }
        }

    }
}
