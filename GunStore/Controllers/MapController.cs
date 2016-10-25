using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ComicBookStore.Models;
using WebGrease.Css.Extensions;

namespace ComicBookStore.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            return View("MapView");
        }

        public string GetAllSellersAddresses()
        {
            List<string> addresses = new List<string>();
            using (ComicsContextDb db = new ComicsContextDb())
            {
                db.Sellers.ForEach(seller => addresses.Add(ReformatSellerAddress(seller)));
            }

            return JsonConvert.SerializeObject(addresses);
        }

        private static string ReformatSellerAddress(Seller seller)
        {
            return seller.City + ", " + seller.Street;
        }
    }
}