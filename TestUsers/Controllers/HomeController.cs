using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestUsers.Models;
using TestUsers.ViewModels;

namespace TestUsers.Controllers
{
    public class HomeController : Controller
    {
        private NavEcommerceDBfirst7Entities db = new NavEcommerceDBfirst7Entities();

        public async Task<ActionResult> Index()
        {
            var motorcycle = db.Motorcycles;
            var brand = db.Brands;
            var imageDataStreetBikes = db.Motorcycles.Where(m => m.Type == "Street").Select(m => m.Image);

            var homeVM = new HomeVM
            {
                MotorcyclesHomeVM = motorcycle.ToList(),
                BrandsHomeVM = brand.ToList(),
                TypeHomeVM = imageDataStreetBikes.ToList()
            };

            return View(homeVM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}