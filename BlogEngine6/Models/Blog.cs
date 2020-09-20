using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.Models
{
    public class Blog
    {

        public int BlogID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Display(Name = "Post Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [StringLength(5000, MinimumLength = 10)]
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}