using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestUsers.Models;

namespace TestUsers.Controllers
{
    public class DealersController : Controller
    {
        private NavEcommerceDBfirstEntities19 db = new NavEcommerceDBfirstEntities19();

        // GET: Dealers
        [Authorize()]
        public async Task<ActionResult> Index()
        {
            return View(await db.Dealers.ToListAsync());
        }

        // GET: Dealers/Details/5
        [Authorize()]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = await db.Dealers.FindAsync(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // GET: Dealers/Create
        [Authorize()]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dealers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DealerId,Name,Address,PhoneNumber")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Dealers.Add(dealer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dealer);
        }

        // GET: Dealers/Edit/5
        [Authorize()]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dealerViewModel = new DealerVM
            {
                Dealer = db.Dealers.Include(i => i.Brands).First(i => i.DealerId == id),
            };

            if (dealerViewModel.Dealer == null)
                return HttpNotFound();

            var allBrandsList = db.Brands.ToList();

            dealerViewModel.AllBrands = allBrandsList.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.BrandId.ToString()
            });

            return View(dealerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DealerVM dealerViewModel)
        {

            if (dealerViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                var dealerToUpdate = db.Dealers
                    .Include(i => i.Brands).First(i => i.DealerId == dealerViewModel.Dealer.DealerId);

                if (TryUpdateModel(dealerToUpdate, "Dealer", new string[] { "Name", "Address", "PhoneNumber", "DealerId" }))
                {
                    var newBrands = db.Brands.Where(
                       m => dealerViewModel.SelectedBrands.Contains(m.BrandId)).ToList();
                    var updatedBrands = new HashSet<int>(dealerViewModel.SelectedBrands);
                    foreach (Brand brand in db.Brands)
                    {
                        if (!updatedBrands.Contains(brand.BrandId))
                        {
                            dealerToUpdate.Brands.Remove(brand);
                        }
                        else
                        {
                            dealerToUpdate.Brands.Add((brand));
                        }
                    }

                    db.Entry(dealerToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(dealerViewModel);
        }

        // GET: Dealers/Delete/5
        [Authorize()]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = await db.Dealers.FindAsync(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dealer dealer = await db.Dealers.FindAsync(id);
            db.Dealers.Remove(dealer);
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
