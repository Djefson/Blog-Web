using BlogEngine6.Models;
using BlogEngine6.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.Controllers
{
    public class BlogBase : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Accept list of blogs, set isFavorite flag for each that user has favorited
        // Return list of blogs with flags set
        public ViewBlogViewModel checkFavorites(ViewBlogViewModel blog)
        {

            // If user is logged in, check if blog is favorited
            string userID = User.Identity.GetUserId();
            if (!String.IsNullOrEmpty(userID))
            {
                FavoriteBlog checkFBlog = db.FavoriteBlogs.SingleOrDefault(b => b.BlogID == blog.BlogID && b.UserID == userID);

                if (checkFBlog != null)
                {
                    blog.isFavorited = true;
                }

            }

            return blog;
        }

        // Accept list of blogs, set isFavorite flag for each that user has favorited
        // Return list of blogs with flags set
        public List<ViewBlogViewModel> checkFavorites(List<ViewBlogViewModel> blogList)
        {

            // If user is logged in, check if blog is favorited
            string userID = User.Identity.GetUserId();
            if (!String.IsNullOrEmpty(userID))
            {

                foreach (var blog in blogList)
                {
                    FavoriteBlog checkFBlog = db.FavoriteBlogs.SingleOrDefault(b => b.BlogID == blog.BlogID && b.UserID == userID);

                    if (checkFBlog != null)
                    {
                        blog.isFavorited = true;
                    }

                }
            }

            return blogList;
        }

        // Accept list of blogs, return list with number comments each blog has
        public List<ViewBlogViewModel> setNumComments(List<ViewBlogViewModel> blogList)
        {
            foreach (var blog in blogList)
            {
                int numComments = db.BlogComments.Where(b => b.BlogID == blog.BlogID).Count();

                if (numComments > 0)
                {
                    blog.commentCount = numComments;
                }

            }
            return blogList;
        }

    }
}