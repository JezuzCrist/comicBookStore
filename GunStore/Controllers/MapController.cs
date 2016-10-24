using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using GunStore.Models;
using WebGrease.Css.Extensions;

namespace GunStore.Controllers
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
                db.Dealers.ForEach(dealer => addresses.Add(ReformatDealerAddress(dealer)));
            }

            return JsonConvert.SerializeObject(addresses);
        }

        private static string ReformatDealerAddress(Seller dealer)
        {
            return dealer.City + ", " + dealer.Street;
        }
    }
}