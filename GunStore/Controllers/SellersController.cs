using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ComicBookStore.Models;

namespace ComicBookStore.Controllers
{
    [Authorize]
    public class SellersController : Controller
    {
        private ComicsContextDb db = new ComicsContextDb();

        // GET: Sellers
        public ActionResult Index()
        {
            return View(db.Sellers.ToList());
        }

        // GET: Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // GET: Sellers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,City,Street,Reliability")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Sellers.Add(seller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seller);
        }

        // GET: Sellers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,City,Street,Reliability")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seller seller = db.Sellers.Find(id);
            db.Sellers.Remove(seller);
            db.SaveChanges();
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

        [AllowAnonymous]
        public ActionResult Search(string firstName, string lastName, string city)
        {
            var sellersFiltered = db.Sellers.Include(x => x.Guns);

            if (!string.IsNullOrEmpty(firstName))
            {
                sellersFiltered = sellersFiltered.Where(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                sellersFiltered = sellersFiltered.Where(x => x.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(city))
            {
                sellersFiltered = sellersFiltered.Where(x => x.City.Contains(city));
            }
            return View(sellersFiltered.ToList());
        }

        public string GroupByReliability()
        {
            var sellers = db.Sellers.GroupBy(seller => seller.Reliability,
                               seller => seller.FirstName + " " + seller.LastName,
                               (key, g) => new {
                                   Reliability = key,
                                   Sellers = g.ToList()
                               }
                              ).ToList();

            var resultSellers = new List<GroupByObject>();
            sellers.ForEach(d => resultSellers.Add(new GroupByObject(d.Reliability, Concat(d.Sellers))));

            return JsonConvert.SerializeObject(resultSellers);
        }

        private static string Concat(IEnumerable<string> source)
        {
            var sb = new StringBuilder();
            foreach (var s in source)
            {
                sb.Append(s + ", ");
            }
            if (sb.Length > 0)
            {
                var commaLength = ", ".Length;
                sb.Remove(sb.Length - commaLength, commaLength);
            }
            return sb.ToString();
        }

        [JsonObject(MemberSerialization.OptIn)]
        private class GroupByObject
        {
            [JsonProperty]
            private int Reliability { get; set; }
            [JsonProperty]
            private string Sellers { get; set; }

            public GroupByObject(int reliability, string sellers)
            {
                Reliability = reliability;
                Sellers = sellers;
            }
        }
    }

}
