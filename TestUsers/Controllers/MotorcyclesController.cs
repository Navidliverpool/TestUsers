using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestUsers.Data.Repositories;
using TestUsers.Models;

namespace TestUsers.Controllers
{
    public class MotorcyclesController : Controller
    {
        private NavEcommerceDBfirstEntities19 db = new NavEcommerceDBfirstEntities19();
        IMotorcycleRepository _motorcycleRepository;
        IBrandRepository _brandRepository;
        IDealerRepository _dealerRepository;
        ICategoryRepository _categoryRepository;

        public MotorcyclesController(IMotorcycleRepository motorcycleRepository,
            IBrandRepository brandRepository,
            IDealerRepository dealerRepository,
            ICategoryRepository categoryRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _brandRepository = brandRepository;
            _dealerRepository = dealerRepository;
            _categoryRepository = categoryRepository;
        }

        public MotorcyclesController()
        {

        }
        // GET: Motorcycles
        [Authorize()]
        public async Task<ActionResult> Index()
        {

            var getAllMotorcyclesIncludeBrandsCategories = _motorcycleRepository.GetMotorcyclesIncludeBrandsCategories();

            return View(await getAllMotorcyclesIncludeBrandsCategories.ToListAsync());
        }

        // GET: Motorcycles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Motorcycle motorcycle = await _motorcycleRepository.GetMotorcycleById(id);

            if (motorcycle == null)
            {
                return HttpNotFound();
            }
            return View(motorcycle);
        }

        // GET: Motorcycles/Create
        [Authorize()]
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(_brandRepository.GetBrands(), "BrandId", "Name");
            ViewBag.CategoryId = new SelectList(_categoryRepository.GetCategories(), "CategoryId", "MotoCategory");
            return View();
        }

        // POST: Motorcycles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MotorcycleId,Model,Price,BrandId,CategoryId")] MotorcycleVM motorcycleToUpdate, HttpPostedFileBase image)
        {

            if (ModelState.IsValid)
            {
                //_motorcycleRepository.AddMotorcycle(motorcycle);

                if (image != null)
                {
                    byte[] data;
                    using (Stream inputStream = image.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    motorcycleToUpdate.Motorcycle.Image = data;
                }

                db.Entry(motorcycleToUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //_motorcycleRepository.SaveChanges();
                //return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "Name", motorcycleToUpdate.Motorcycle.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "MotoCategory", motorcycleToUpdate.Motorcycle.CategoryId);
            return View(motorcycleToUpdate);
        }

        // GET: Motorcycles/Edit/5
        [Authorize()]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var motorcycleViewModel = new MotorcycleVM
            {
                Motorcycle = _motorcycleRepository.GetMotorcycleIncludeItsDealers(id)
            };

            if (motorcycleViewModel.Motorcycle == null)
                return HttpNotFound();

            var allDealersList = _dealerRepository.GetDealers();

            motorcycleViewModel.AllDealers = allDealersList.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DealerId.ToString()
            });


            var imageData = db.Motorcycles.Where(m => m.Image == motorcycleViewModel.Motorcycle.Image).FirstOrDefault();

            if (imageData != null)
            {
                motorcycleViewModel.Motorcycle.Image = imageData.Image;
            }

            ViewBag.BrandId =
                    new SelectList(db.Brands, "BrandId", "Name", motorcycleViewModel.Motorcycle.BrandId);

            ViewBag.categoryId =
                new SelectList(db.Categories, "CategoryId", "MotoCategory", motorcycleViewModel.Motorcycle.CategoryId);

            return View(motorcycleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MotorcycleVM motorcycleViewModel, HttpPostedFileBase image)
        {

            if (motorcycleViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {

                var motorcycleToUpdate = db.Motorcycles
                    .Include(m => m.Dealers).First(m => m.MotorcycleId == motorcycleViewModel.Motorcycle.MotorcycleId);

                if (TryUpdateModel(motorcycleToUpdate, "Motorcycle", new string[] { "Model", "Price", "Image", "BrandId", "Dealers", "CategoryId", "MotorcycleId" }))
                {
                    var newDealers = db.Dealers.Where(
                       m => motorcycleViewModel.SelectedDealers.Contains(m.DealerId)).ToList();
                    var updatedDealers = new HashSet<int>(motorcycleViewModel.SelectedDealers);
                    foreach (Dealer dealer in db.Dealers)
                    {
                        if (!updatedDealers.Contains(dealer.DealerId))
                        {
                            motorcycleToUpdate.Dealers.Remove(dealer);
                        }
                        else
                        {
                            motorcycleToUpdate.Dealers.Add((dealer));
                        }
                    }

                    if (image != null)
                    {
                        byte[] data;
                        using (Stream inputStream = image.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                        }

                        motorcycleToUpdate.Image = data;
                    }

                    db.Entry(motorcycleToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
            }


            var allDealersList = db.Dealers.ToList();

            var brand = db.Brands.FirstOrDefault(b => b.BrandId == motorcycleViewModel.Motorcycle.BrandId);

            motorcycleViewModel.Motorcycle.Brand = brand;

            var category = db.Categories.FirstOrDefault(c => c.CategoryId == motorcycleViewModel.Motorcycle.CategoryId);

            motorcycleViewModel.Motorcycle.Category = category;

            motorcycleViewModel.AllDealers = allDealersList.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DealerId.ToString()
            });

            ViewBag.BrandId =
                    new SelectList(db.Brands, "BrandId", "Name", motorcycleViewModel.Motorcycle.BrandId);

            ViewBag.CategoryId =
                    new SelectList(db.Categories, "CategoryId", "MotoCategory", motorcycleViewModel.Motorcycle.CategoryId);
            return View(motorcycleViewModel);
        }


        // GET: Motorcycles/Delete/5
        [Authorize()]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motorcycle motorcycle = await db.Motorcycles.FindAsync(id);
            if (motorcycle == null)
            {
                return HttpNotFound();
            }
            return View(motorcycle);
        }

        // POST: Motorcycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Motorcycle motorcycle = await db.Motorcycles.FindAsync(id);
            db.Motorcycles.Remove(motorcycle);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
