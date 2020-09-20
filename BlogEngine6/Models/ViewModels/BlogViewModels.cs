using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.Models.ViewModels
{
    public class CreateBlogViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [StringLength(5000, MinimumLength = 10)]
        public string Content { get; set; }
    }

    public class ViewBlogViewModel
    {

        public int BlogID { get; set; }

        [Display(Name = "Author")]
        public string UserName { get; set; }

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

        public bool isFavorited { get; set; }
        public int commentCount { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public ViewBlogViewModel()
        {
            isFavorited = false;
            commentCount = 0;
        }

    }

    public class FeaturedPostViewModel
    {
        [Required]
        public int BlogID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }
    }

    public class CreateBlogCommentViewModel
    {
        [Required]
        public int BlogID { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 3)]
        public string Message { get; set; }

    }

    public class ViewBlogCommentViewModel
    {
        [Required]
        public int BlogCommentID { get; set; }

        [Required]
        public string Author { get; set; }

        [Display(Name = "Post Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 3)]
        public string Message { get; set; }

    }

}