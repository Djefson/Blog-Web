using BlogEngine6.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BlogEngine6.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace BlogEngine6.Controllers
{
    public class BlogsController : BlogBase
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        const int MAX_COMMENTS_PER_BLOG = 3;

        // GET: Blogs
        public ActionResult Index(string author, string tag, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentAuthor = author;
            ViewBag.CurrentTag = (String.IsNullOrEmpty(tag) ? null : tag);

            // Allow for paging and search filter
            if (searchString != null) {
                page = 1;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = (String.IsNullOrEmpty(searchString) ? null : searchString);

            var blogs = db.Blogs.Include(b => b.User);

            // Add search string to query
            if (!String.IsNullOrEmpty(searchString)) {
                blogs = blogs.Where(b => b.User.UserName.Contains(searchString) || b.Title.Contains(searchString));
            }

            // Add author to query
            if (!String.IsNullOrEmpty(author)) {
                blogs = blogs.Where(b => b.User.UserName == author);
            }

            // Add tag to query
            if (!String.IsNullOrEmpty(tag))
            {
                blogs = blogs.Where(x => x.Tags.Any(p2 => p2.Name == tag));

            }

            blogs = blogs.OrderByDescending(b => b.PostDate);

            List<ViewBlogViewModel> blogList = blogs.AsEnumerable()
                          .Select(b => new ViewBlogViewModel
                          {
                              BlogID = b.BlogID,
                              UserName = b.User.UserName,
                              PostDate = b.PostDate,
                              Title = b.Title,
                              Content = b.Content,
                              Tags = b.Tags,
                              isFavorited = false,
                              commentCount = 0
                          }).ToList();

            blogList = checkFavorites(blogList);
            blogList = setNumComments(blogList);

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(blogList.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            ViewBlogViewModel blogViewModel = new ViewBlogViewModel
            {
                BlogID = blog.BlogID,
                UserName = blog.User.UserName,
                PostDate = blog.PostDate,
                Title = blog.Title,
                Content = blog.Content
            };

            blogViewModel = checkFavorites(blogViewModel);
            return View(blogViewModel);
        }

        public async Task<ActionResult> DetailsJSON(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = await db.Blogs.FindAsync(id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            var blogJSON = new Dictionary<string, string>();

            blogJSON["Title"] = blog.Title;
            blogJSON["PostDate"] = blog.PostDate.ToString();

            return Json(blogJSON, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FavoritePost(int blogID)
        {
            string userID = User.Identity.GetUserId();
            FavoriteBlog checkFBlog = db.FavoriteBlogs.SingleOrDefault(b => b.BlogID == blogID && b.UserID == userID);

            try
            {
                // If post is already favorited, remove favorite
                if (checkFBlog != null)
                {
                    db.FavoriteBlogs.Remove(checkFBlog);
                    db.SaveChanges();

                    return Json(new { success = true, isAdded = false, blogId = blogID });
                }

                FavoriteBlog fBlog = new FavoriteBlog();

                fBlog.BlogID = blogID;
                fBlog.UserID = userID;
                db.FavoriteBlogs.Add(fBlog);

                await db.SaveChangesAsync();

                return Json(new { success = true, isAdded = true, blogId = blogID });
                
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "An unknown error occured." });
            }
        }

        [Authorize]
        public ActionResult Favorites(int? page)
        {
            // Get all user favorited blogs
            string userID = User.Identity.GetUserId();
            var fBlogs = db.FavoriteBlogs.Where(b => b.UserID == userID).OrderByDescending(b => b.Blog.PostDate);

            List<ViewBlogViewModel> blogList = fBlogs.AsEnumerable()
                          .Select(b => new ViewBlogViewModel
                          {
                              BlogID = b.BlogID,
                              UserName = b.User.UserName,
                              PostDate = b.Blog.PostDate,
                              Title = b.Blog.Title,
                              Content = b.Blog.Content,
                              Tags = b.Blog.Tags,
                              isFavorited = true
                          }).ToList();

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostComment([Bind(Include = "BlogID,Message")] CreateBlogCommentViewModel comment)
        {
            try
            {
                string userID = User.Identity.GetUserId();
                var commentCount = db.BlogComments.Where(b => b.BlogID == comment.BlogID && b.UserID == userID).Count();

                if(commentCount > MAX_COMMENTS_PER_BLOG)
                {
                    return Json(new { success = false, msg = "Comment cannot be posted. You can only post a maximum of " + MAX_COMMENTS_PER_BLOG + " comments per blog." });
                }

                BlogComment newComment = new BlogComment();

                if (ModelState.IsValid)
                {
                    newComment.BlogID = comment.BlogID;
                    newComment.Message = comment.Message;
                    newComment.UserID = userID;
                    newComment.PostDate = DateTime.Now;
                    db.BlogComments.Add(newComment);

                    await db.SaveChangesAsync();

                    return Json(new { success = true, blogId = comment.BlogID });
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return Json(new { success = false, msg = "Comment not posted. An unknown error occured." });
        }


        [ChildActionOnly]
        public ActionResult FeaturedPosts()
        {
            var featuredBlogs = db.FeaturedBlogs
                                .OrderBy(b => b.FeatureDate).Take(5);

            List<FeaturedPostViewModel> blogList = featuredBlogs.AsEnumerable()
                          .Select(b => new FeaturedPostViewModel
                          {
                              BlogID = b.BlogID,
                              Title = b.Blog.Title,
                              Author = b.Blog.User.UserName
                          }).ToList();

            ViewBag.Title = "Editors’ picks";
            ViewBag.Description = "Stories that matter.";

            return PartialView("SidebarPostsPartial", blogList);
        }

        [ChildActionOnly]
        public ActionResult RandomPosts()
        {
            // Get random blog based on generated GUID
            var featuredBlogs = db.Blogs
                                .OrderBy(b => b.PostDate)
                                .OrderBy(r => Guid.NewGuid()).Take(5);

            List<FeaturedPostViewModel> blogList = featuredBlogs.AsEnumerable()
                          .Select(b => new FeaturedPostViewModel
                          {
                              BlogID = b.BlogID,
                              Title = b.Title,
                              Author = b.User.UserName
                          }).ToList();

            ViewBag.Title = "Reading roulette";
            ViewBag.Description = "The new variety hour.";

            return PartialView("SidebarPostsPartial", blogList);
        }

        [ChildActionOnly]
        public ActionResult FavoriteStories()
        {

            // Get top occuring blogs from favorite blogs table
            var favBlogs = (from blogs in db.FavoriteBlogs
                           group blogs by blogs.BlogID into blogGroup
                           select new FeaturedPostViewModel
                           {
                               BlogID = blogGroup.Key,
                               Title = blogGroup.Select(b => b.Blog.Title).FirstOrDefault(),
                               Author = blogGroup.Select(b => b.Blog.User.UserName).FirstOrDefault(),
                           }).Take(5).ToList();

            ViewBag.Title = "Top Stories";
            ViewBag.Description = "What people love.";

            return PartialView("SidebarPostsPartial", favBlogs);
        }

        [ChildActionOnly]
        public ActionResult BlogTags()
        {
            // Get random blog based on generated GUID
            var tags = db.Tags.OrderBy(b => b.Name);

            return PartialView("SidebarTagsPartial", tags);
        }


        public ActionResult BlogComments(int id, int count)
        {
            // Get random blog based on generated GUID
            var comments = db.BlogComments.Where(b => b.BlogID == id).OrderByDescending(b => b.PostDate).Take(count);

            List<ViewBlogCommentViewModel> cList = comments.AsEnumerable()
                          .Select(b => new ViewBlogCommentViewModel
                          {
                              BlogCommentID = b.BlogCommentID,
                              Author = b.User.UserName,
                              PostDate = b.PostDate,
                              Message = b.Message
                          }).ToList();

            ViewBag.BlogID = id;
            ViewBag.Count = count;
            return PartialView("BlogCommentsPartial", cList);
        }

        [ChildActionOnly]
        public ActionResult BlogCommentsForm(int id, int count)
        {
            ViewBag.BlogID = id;
            ViewBag.Count = count;
            return PartialView("BlogCommentsFormPartial");
        }

    }
}