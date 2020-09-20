using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogEngine6.Models
{
    public class BlogComment
    {

        public int BlogCommentID { get; set; }

        [Required]
        public int BlogID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Display(Name = "Post Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 3)]
        public string Message { get; set; }


        public virtual Blog Blog { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}