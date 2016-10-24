using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GunStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Gun")]
        public int GunID { get; set; }
        public Rank GunRank { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public Gun Gun { get; set; }
        public DateTime PublicityDate { get; set; }

        public enum Rank
        {
            [Display(Name = "Very bad")]
            VERY_BAD = 1,
            [Display(Name = "Bad")]
            BAD,
            [Display(Name = "Ok")]
            OK,
            [Display(Name = "Good")]
            GOOD,
            [Display(Name = "Very good")]
            VERY_GOOD
        }
    }
}