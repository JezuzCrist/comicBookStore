using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComicBookStore.Models;
using System.IO;

namespace ComicBookStore.Controllers
{
    public class ComicsController : Controller
    {
        private ComicsContextDb db = new ComicsContextDb();

        // GET: Comics
        public ActionResult Index()
        {
            var comics = db.Comics.Include(g => g.Seller);
            return View(comics.ToList());
        }

        // GET: Comics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comics comics = db.Comics.Find(id);
            if (comics == null)
            {
                return HttpNotFound();
            }
            return View(comics);
        }

        // GET: Comics/Create
        public ActionResult Create()
        {
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "FirstName");
            return View();
        }

        // POST: Comics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SellerId,Name,Description,Price,Genre")] Comics comics, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                comics.IsPhotoExists = file != null && file.ContentLength > 0;
                db.Comics.Add(comics);
                db.SaveChanges();

                if (comics.IsPhotoExists)
                {
                    string fileName = comics.Id + ".png";
                    string newImage = Path.Combine(Server.MapPath("/Uploads"), fileName);
                    file.SaveAs(newImage);
                }

                return RedirectToAction("Index");
            }

            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "FirstName", comics.SellerId);
            return View(comics);
        }

        // GET: Comics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comics comics = db.Comics.Find(id);
            if (comics == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "FirstName", comics.SellerId);
            return View(comics);
        }

        // POST: Comics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SellerId,Name,Description,Price,Genre,IsPhotoExists")] Comics comics, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = comics.Id + ".png";
                    string newImage = Path.Combine(Server.MapPath("/Uploads"), fileName);

                    if (comics.IsPhotoExists)
                    {
                        System.IO.File.Delete(newImage);
                    }
                    else
                    {
                        comics.IsPhotoExists = true;
                    }

                    file.SaveAs(newImage);
                }

                db.Entry(comics).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.SellerId = new SelectList(db.Sellers, "Id", "FirstName", comics.SellerId);
            return View(comics);
        }

        // GET: Comics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comics comics = db.Comics.Find(id);
            if (comics == null)
            {
                return HttpNotFound();
            }
            return View(comics);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comics comics = db.Comics.Find(id);

            if (comics.IsPhotoExists)
            {
                string fileName = comics.Id + ".png";
                string delImage = Path.Combine(Server.MapPath("/Uploads"), fileName);
                System.IO.File.Delete(delImage);
            }

            db.Comics.Remove(comics);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Search(string comicsName, int? minPrice, int? maxPrice, string gener)
        {
            var comicsQuery = db.Comics.Include(x => x.Reviews);

            if (!string.IsNullOrEmpty(comicsName))
            {
                comicsQuery = comicsQuery.Where(x => x.Name.Contains(comicsName));
            }

            if (minPrice.HasValue)
            {
                if (minPrice >= 0)
                {
                    comicsQuery = comicsQuery.Where(x => x.Price >= minPrice);
                }
            }

            if (maxPrice.HasValue)
            {
                if (maxPrice >= 0)
                {
                    comicsQuery = comicsQuery.Where(x => x.Price <= maxPrice);
                }
            }

            if (!string.IsNullOrEmpty(gener))
            {
                comicsQuery = comicsQuery.Where(x => x.Genre.Contains(gener));
            }

            ViewBag.Comics = comicsQuery.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddReview(Review.Rank ComicsRank, string reviewTitle, string reviewAuthor, string reviewContent, string comicsId)
        {
            try
            {
                Review newReview = new Review
                {
                    ComicsRank = ComicsRank,
                    Title = reviewTitle,
                    Author = reviewAuthor,
                    Content = reviewContent,
                    ComicsID = int.Parse(comicsId),
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