using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GunStore.Models;
using System.IO;

namespace GunStore.Controllers
{
    public class GunsController : Controller
    {
        private ComicsContextDb db = new ComicsContextDb();

        // GET: Guns
        public ActionResult Index()
        {
            var guns = db.Guns.Include(g => g.Dealer);
            return View(guns.ToList());
        }

        // GET: Guns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gun gun = db.Guns.Find(id);
            if (gun == null)
            {
                return HttpNotFound();
            }
            return View(gun);
        }

        // GET: Guns/Create
        public ActionResult Create()
        {
            ViewBag.SellerId = new SelectList(db.Dealers, "Id", "FirstName");
            return View();
        }

        // POST: Guns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SellerId,Name,Description,Price,Genre")] Gun gun, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                gun.IsPhotoExists = file != null && file.ContentLength > 0;
                db.Guns.Add(gun);
                db.SaveChanges();

                if (gun.IsPhotoExists)
                {
                    string fileName = gun.Id + ".png";
                    string newImage = Path.Combine(Server.MapPath("/Uploads"), fileName);
                    file.SaveAs(newImage);
                }

                return RedirectToAction("Index");
            }

            ViewBag.SellerId = new SelectList(db.Dealers, "Id", "FirstName", gun.SellerId);
            return View(gun);
        }

        // GET: Guns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gun gun = db.Guns.Find(id);
            if (gun == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellerId = new SelectList(db.Dealers, "Id", "FirstName", gun.SellerId);
            return View(gun);
        }

        // POST: Guns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SellerId,Name,Description,Price,Genre,IsPhotoExists")] Gun gun, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = gun.Id + ".png";
                    string newImage = Path.Combine(Server.MapPath("/Uploads"), fileName);

                    if (gun.IsPhotoExists)
                    {
                        System.IO.File.Delete(newImage);
                    }
                    else
                    {
                        gun.IsPhotoExists = true;
                    }

                    file.SaveAs(newImage);
                }

                db.Entry(gun).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.SellerId = new SelectList(db.Dealers, "Id", "FirstName", gun.SellerId);
            return View(gun);
        }

        // GET: Guns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gun gun = db.Guns.Find(id);
            if (gun == null)
            {
                return HttpNotFound();
            }
            return View(gun);
        }

        // POST: Guns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gun gun = db.Guns.Find(id);

            if (gun.IsPhotoExists)
            {
                string fileName = gun.Id + ".png";
                string delImage = Path.Combine(Server.MapPath("/Uploads"), fileName);
                System.IO.File.Delete(delImage);
            }

            db.Guns.Remove(gun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Search(string gunName, int? minPrice, int? maxPrice, string gener)
        {
            var gunsQuery = db.Guns.Include(x => x.Reviews);

            if (!string.IsNullOrEmpty(gunName))
            {
                gunsQuery = gunsQuery.Where(x => x.Name.Contains(gunName));
            }

            if (minPrice.HasValue)
            {
                if (minPrice >= 0)
                {
                    gunsQuery = gunsQuery.Where(x => x.Price >= minPrice);
                }
            }

            if (maxPrice.HasValue)
            {
                if (maxPrice >= 0)
                {
                    gunsQuery = gunsQuery.Where(x => x.Price <= maxPrice);
                }
            }

            if (!string.IsNullOrEmpty(gener))
            {
                gunsQuery = gunsQuery.Where(x => x.Genre.Contains(gener));
            }

            ViewBag.Guns = gunsQuery.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddReview(Review.Rank GunRank, string reviewTitle, string reviewAuthor, string reviewContent, string gunId)
        {
            try
            {
                Review newReview = new Review
                {
                    GunRank = GunRank,
                    Title = reviewTitle,
                    Author = reviewAuthor,
                    Content = reviewContent,
                    GunID = int.Parse(gunId),
                    PublicityDate = DateTime.Now
                };

                db.Reviews.Add(newReview);
                db.SaveChanges();

                return RedirectToAction("Search");
            }
            catch (Exception)
            {
                return RedirectToAction("Search");
            }
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