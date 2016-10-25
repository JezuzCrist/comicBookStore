using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComicBookStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Comics")]
        public int ComicsID { get; set; }
        public Rank ComicsRank { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public Comics Comics { get; set; }
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