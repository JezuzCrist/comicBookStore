using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GunStore.Models
{
    public class Comics
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Seller")]
        public int? SellerId { get; set; }

        [DisplayName("Title")]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "US ${0:0.00}", ApplyFormatInEditMode = false)]
        public double Price { get; set; }

        [DisplayName("Genre")]
        public string Genre { get; set; }
        public bool IsPhotoExists { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public Seller Seller { get; set; }

        public Comics()
        {
        }
    }
}