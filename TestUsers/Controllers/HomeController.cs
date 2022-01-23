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
            var streetBikes = db.Motorcycles.Where(m => m.Category.MotoCategory == "Street");
            var sportBikes = db.Motorcycles.Where(m => m.Category.MotoCategory == "Sport");
            var adventureBikes = db.Motorcycles.Where(m => m.Category.MotoCategory == "Adventure");
            var scooterBikes = db.Motorcycles.Where(m => m.Category.MotoCategory == "Scooter");

            var homeVM = new HomeVM
            {
                MotorcyclesHomeVM = motorcycle.ToList(),
                BrandsHomeVM = brand.ToList(),
                StreetBikesHomeVM = streetBikes.ToList(),
                SportBikesHomeVM = sportBikes.ToList(),
                AdventureBikesHomeVM = adventureBikes.ToList(),
                ScooterBikesHomeVM = scooterBikes.ToList()                
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