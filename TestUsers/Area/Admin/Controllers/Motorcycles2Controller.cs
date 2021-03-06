//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using TestUsers.Models;

//namespace test.Controllers
//{
//    public class Motorcycles2Controller : Controller
//    {
//        private NavEcommerceDBfirstEntities17 db = new NavEcommerceDBfirstEntities17();

//        // GET: Motorcycles
//        [Authorize()]
//        public async Task<ActionResult> Index()
//        {

//            var motorcycles = db.Motorcycles.Include(m => m.Brand);
//            return View(await motorcycles.ToListAsync());
//        }

//        // GET: Motorcycles/Details/5
//        public async Task<ActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Motorcycle motorcycle = await db.Motorcycles.FindAsync(id);
//            if (motorcycle == null)
//            {
//                return HttpNotFound();
//            }
//            return View(motorcycle);
//        }

//        // GET: Motorcycles/Create
//        [Authorize()]
//        public ActionResult Create()
//        {

//            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "Name");
//            return View();
//        }

//        // POST: Motorcycles/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create([Bind(Include = "MotorcycleId,Model,Price,BrandId")] Motorcycle motorcycle)
//        {

//            if (ModelState.IsValid)
//            {
//                db.Motorcycles.Add(motorcycle);
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }

//            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "Name", motorcycle.BrandId);
//            return View(motorcycle);
//        }

//        // GET: Motorcycles/Edit/5
//        [Authorize()]
//        public async Task<ActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            var motorcycleViewModel = new MotorcycleVM
//            {
//                Motorcycle = db.Motorcycles.Include(i => i.Dealers).First(i => i.MotorcycleId == id),
//            };

//            if (motorcycleViewModel.Motorcycle == null)
//                return HttpNotFound();

//            var allDealersList = db.Dealers.ToList();

//            motorcycleViewModel.AllDealers = allDealersList.Select(d => new SelectListItem
//            {
//                Text = d.Name,
//                Value = d.DealerId.ToString()
//            });


//            var imageData = db.Motorcycles.Where(m => m.Image == motorcycleViewModel.Motorcycle.Image).FirstOrDefault();

//            if (imageData != null)
//            {
//                motorcycleViewModel.Motorcycle.Image = imageData.Image;
//            }

//            ViewBag.BrandId =
//                    new SelectList(db.Brands, "BrandId", "Name", motorcycleViewModel.Motorcycle.BrandId);

//            return View(motorcycleViewModel);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(MotorcycleVM motorcycleViewModel, HttpPostedFileBase image)
//        {

//            if (motorcycleViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

//            if (ModelState.IsValid)
//            {

//                var motorcycleToUpdate = db.Motorcycles
//                    .Include(m => m.Dealers).First(m => m.MotorcycleId == motorcycleViewModel.Motorcycle.MotorcycleId);

//                if (TryUpdateModel(motorcycleToUpdate, "Motorcycle", new string[] { "Model", "Price", "Image", "BrandId", "Dealers", "MotorcycleId" }))
//                {
//                    var newDealers = db.Dealers.Where(
//                       m => motorcycleViewModel.SelectedDealers.Contains(m.DealerId)).ToList();
//                    var updatedDealers = new HashSet<int>(motorcycleViewModel.SelectedDealers);
//                    foreach (Dealer dealer in db.Dealers)
//                    {
//                        if (!updatedDealers.Contains(dealer.DealerId))
//                        {
//                            motorcycleToUpdate.Dealers.Remove(dealer);
//                        }
//                        else
//                        {
//                            motorcycleToUpdate.Dealers.Add((dealer));
//                        }
//                    }

//                    if (image != null)
//                    {
//                        byte[] data;
//                        using (Stream inputStream = image.InputStream)
//                        {
//                            MemoryStream memoryStream = inputStream as MemoryStream;
//                            if (memoryStream == null)
//                            {
//                                memoryStream = new MemoryStream();
//                                inputStream.CopyTo(memoryStream);
//                            }
//                            data = memoryStream.ToArray();
//                        }

//                        motorcycleToUpdate.Image = data;
//                    }

//                    db.Entry(motorcycleToUpdate).State = System.Data.Entity.EntityState.Modified;
//                    db.SaveChanges();

//                }
//            }


//            var allDealersList = db.Dealers.ToList();

//            var brand = db.Brands.FirstOrDefault(b => b.BrandId == motorcycleViewModel.Motorcycle.BrandId);

//            motorcycleViewModel.Motorcycle.Brand = brand;

//            motorcycleViewModel.AllDealers = allDealersList.Select(d => new SelectListItem
//            {
//                Text = d.Name,
//                Value = d.DealerId.ToString()
//            });

//            ViewBag.BrandId =
//                    new SelectList(db.Brands, "BrandId", "Name", motorcycleViewModel.Motorcycle.BrandId);
//            return View(motorcycleViewModel);
//        }


//        // GET: Motorcycles/Delete/5
//        [Authorize()]
//        public async Task<ActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Motorcycle motorcycle = await db.Motorcycles.FindAsync(id);
//            if (motorcycle == null)
//            {
//                return HttpNotFound();
//            }
//            return View(motorcycle);
//        }

//        // POST: Motorcycles/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(int id)
//        {
//            Motorcycle motorcycle = await db.Motorcycles.FindAsync(id);
//            db.Motorcycles.Remove(motorcycle);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
