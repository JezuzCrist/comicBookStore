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

        public string GetAllDealersAddresses()
        {
            List<string> addresses = new List<string>();
            using (ComicsContextDb db = new ComicsContextDb())
            {
                db.Sellers.ForEach(dealer => addresses.Add(ReformatDealerAddress(dealer)));
            }

            return JsonConvert.SerializeObject(addresses);
        }

        private static string ReformatDealerAddress(Seller dealer)
        {
            return dealer.City + ", " + dealer.Street;
        }
    }
}