using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogEngine6.Models
{
    public class FeaturedBlog
    {
        [Key]
        public Guid FeaturedBlogID { set; get; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public int BlogID { get; set; }

        [Display(Name = "Feature Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime FeatureDate { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("BlogID")]
        public virtual Blog Blog { get; set; }
    }
}