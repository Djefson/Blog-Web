using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogEngine6.Models
{
    public class FavoriteBlog
    {

        public int FavoriteBlogID { get; set; }

        [Required]
        public int BlogID { get; set; }

        [Required]
        public string UserID { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}