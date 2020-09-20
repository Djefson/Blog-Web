using BlogEngine6.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.Controllers
{
    public class EditorController : Controller
    {

        const int MAX_USER_FEATURE_COUNT = 5;
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Editor")]
        public ActionResult Index()
        {
            // Return this users featured posts
            var userID = User.Identity.GetUserId();
            var featuredBlogs = db.FeaturedBlogs.Where(b => b.UserID == userID);
            featuredBlogs = featuredBlogs.OrderBy(b => b.FeatureDate);

            return View(featuredBlogs.ToList());
        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeaturedArticle(int? id)
        {

            // Check ID and blog
            if (id == null)
            {
                return Json(new { success = false, msg = "This blog doesn't exist."});
            }

            Blog blog = db.Blogs.Find(id);

            if (blog == null)
            {
                return Json(new { success = false, msg = "This blog doesn't exist." });
            }


            // Check max feature count
            var userID = User.Identity.GetUserId();
            int userFeaturedBlogsCount = db.FeaturedBlogs.Where(b => b.UserID == userID).Count();

            if (userFeaturedBlogsCount >= MAX_USER_FEATURE_COUNT)
            {
                return Json(new { success = false, msg = "You cannot feature more then " + MAX_USER_FEATURE_COUNT + " posts." });
            }


            // Make sure blog isnt already featured
            FeaturedBlog checkFBlog = db.FeaturedBlogs.SingleOrDefault(b => b.BlogID == id);

            if (checkFBlog != null)
            {
                return Json(new { success = false, msg= "This post is already featured!" });
            }


            // Add featured blog to DB
            FeaturedBlog fBlog = new FeaturedBlog
            {
                FeaturedBlogID = Guid.NewGuid(),
                UserID = userID,
                BlogID = blog.BlogID,
                FeatureDate = DateTime.Now
            };

            try
            {
                db.FeaturedBlogs.Add(fBlog);

                db.SaveChanges();

                return Json(new { success = true, msg = "Post successfully featured!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "An unknown error occured." });
            }

        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFeaturedArticle(int id)
        {
            var userID = User.Identity.GetUserId();
            FeaturedBlog fBlog = db.FeaturedBlogs.SingleOrDefault(b => b.BlogID == id);

            if (fBlog.UserID == userID && fBlog != null)
            {
                db.FeaturedBlogs.Remove(fBlog);
                db.SaveChanges();

                return Json(new { success = true, blogId = id });
            }

            return Json(new { success = false });
        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveComment(int id)
        {
            BlogComment comment = db.BlogComments.SingleOrDefault(b => b.BlogCommentID == id);

            if(comment != null)
            {
                db.BlogComments.Remove(comment);
                db.SaveChanges();
                return Json(new { success = true, BlogCommentID = id });  
            }
            return Json(new { success = false});
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}