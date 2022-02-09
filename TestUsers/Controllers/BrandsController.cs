using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestUsers.Models;

namespace test.Controllers
{
    public class BrandsController : Controller
    {
        private NavEcommerceDBfirstEntities19 db = new NavEcommerceDBfirstEntities19();

        // GET: Brands
        [Authorize()]
        public async Task<ActionResult> Index()
        {
            return View(await db.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        [Authorize()]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BrandId,Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Dealers/Edit/5
        [Authorize()]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var brandViewModel = new BrandVM
            {
                Brand = db.Brands.Include(i => i.Dealers).Include(i => i.Motorcycles).First(i => i.BrandId == id),

            };

            if (brandViewModel.Brand == null)
                return HttpNotFound();

            var allDealersList = db.Dealers.ToList();
            brandViewModel.AllDealers = allDealersList.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DealerId.ToString()
            });

            var allMotocyclesList = db.Motorcycles.ToList();
            brandViewModel.AllMotorcycles = allMotocyclesList.Select(m => new SelectListItem
            {
                Text = m.Model,
                Value = m.MotorcycleId.ToString()
            });

            var imageData = db.Brands.Where(m => m.Image == brandViewModel.Brand.Image).FirstOrDefault();

            if (imageData != null)
            {
                brandViewModel.Brand.Image = imageData.Image;
            }

            return View(brandViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BrandVM brandViewModel, HttpPostedFileBase image)
        {

            if (brandViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                var brandToUpdate = db.Brands
                    .Include(i => i.Dealers).Include(i => i.Motorcycles).First(i => i.BrandId == brandViewModel.Brand.BrandId);

                if (TryUpdateModel(brandToUpdate, "Brand", new string[] { "Name", "Image", "BrandId", "Dealers", "Motorcycles" }))
                {
                    var newDealer = db.Dealers.Where(
                       m => brandViewModel.SelectedDealers.Contains(m.DealerId)).ToList();
                    var updatedDealers = new HashSet<int>(brandViewModel.SelectedDealers);
                    foreach (Dealer dealer in db.Dealers)
                    {
                        if (!updatedDealers.Contains(dealer.DealerId))
                        {
                            brandToUpdate.Dealers.Remove(dealer);
                        }
                        else
                        {
                            brandToUpdate.Dealers.Add((dealer));
                        }
                    }

                    var newMotorcycle = db.Motorcycles.Where(
                        m => brandViewModel.SelectedMotorcycles.Contains(m.MotorcycleId)).ToList();
                    var updatedMotorcycles = new HashSet<int>(brandViewModel.SelectedMotorcycles);
                    foreach (Motorcycle motorcycle in db.Motorcycles)
                    {
                        if (!updatedMotorcycles.Contains(motorcycle.MotorcycleId))
                        {
                            brandToUpdate.Motorcycles.Remove(motorcycle);
                        }
                        else
                        {
                            brandToUpdate.Motorcycles.Add((motorcycle));
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

                        brandToUpdate.Image = data;
                    }

                    db.Entry(brandToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            var allDealersList = db.Dealers.ToList();
            brandViewModel.AllDealers = allDealersList.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DealerId.ToString()
            });

            var allMotorcyclesList = db.Motorcycles.ToList();
            brandViewModel.AllMotorcycles = allMotorcyclesList.Select(d => new SelectListItem
            {
                Text = d.Model,
                Value = d.MotorcycleId.ToString()
            });
            return View(brandViewModel);
        }

        // GET: Brands/Delete/5
        [Authorize()]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Brand brand = await db.Brands.FindAsync(id);
            db.Brands.Remove(brand);
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
