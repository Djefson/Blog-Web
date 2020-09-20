using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogEngine6.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using BlogEngine6.Models.ViewModels;
using PagedList;

namespace BlogEngine6.Controllers
{
    public class MyBlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        const int MAX_BLOG_TAGS = 3;

        // GET: MyBlog
        [Authorize]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            // Allow for paging and search filter
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = (String.IsNullOrEmpty(searchString) ? null : searchString);

            var userID = User.Identity.GetUserId();
            var blogs = db.Blogs.Include(b => b.User).Where(b => b.UserID == userID);

            // Add search string to query
            if (!String.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(b => b.User.UserName.Contains(searchString) || b.Title.Contains(searchString));
            }


            blogs = blogs.OrderByDescending(b => b.PostDate);

            List<ViewBlogViewModel> blogList = blogs.AsEnumerable()
                          .Select(o => new ViewBlogViewModel
                          {
                              BlogID = o.BlogID,
                              UserName = o.User.UserName,
                              PostDate = o.PostDate,
                              Title = o.Title,
                              Content = o.Content,
                              Tags = o.Tags
                          }).ToList();

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);
            var userID = User.Identity.GetUserId();

            if (blog == null)
            {
                return HttpNotFound();
            }
            else if (blog.UserID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ViewBlogViewModel blogViewModel = new ViewBlogViewModel{
                                        BlogID = blog.BlogID,
                                        UserName = blog.User.UserName,
                                        PostDate = blog.PostDate,
                                        Title = blog.Title,
                                        Content = blog.Content
                                    };

            return View(blogViewModel);
        }


        // GET: MyBlog/Create
        [Authorize]
        public ActionResult Create()
        {

            IEnumerable<Tag> tags = db.Tags.OrderBy(b => b.Name ).ToList();
            ViewBag.BlogTags = tags;

            return View();
        }

       
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Content")] CreateBlogViewModel blog, string[] selectedTags)
        {
           try
            {
                Blog newBlog = new Blog();

                if (selectedTags != null)
                {

                    if (selectedTags.Length > MAX_BLOG_TAGS)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    newBlog.Tags = new List<Tag>();
                    foreach (var tag in selectedTags)
                    {
                        var taggtoAdd = db.Tags.Find(int.Parse(tag));
                        newBlog.Tags.Add(taggtoAdd);
                    }
                }

                if (ModelState.IsValid)
                {
                    newBlog.Title = blog.Title;
                    newBlog.Content = blog.Content;
                    newBlog.UserID = User.Identity.GetUserId();
                    newBlog.PostDate = DateTime.Now;
                    db.Blogs.Add(newBlog);

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(blog);
        }


        // GET: MyBlog/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);
            var userID = User.Identity.GetUserId();

            if (blog == null)
            {
                return HttpNotFound();
            }
            else if (blog.UserID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBlogViewModel blogViewModel = new ViewBlogViewModel
            {
                BlogID = blog.BlogID,
                UserName = blog.User.UserName,
                PostDate = blog.PostDate,
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags
            };

            IEnumerable<Tag> tags = db.Tags.OrderBy(b => b.Name).ToList();
            ViewBag.BlogTags = tags;

            return View(blogViewModel);
        }
        
        [Authorize]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedTags)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userID = User.Identity.GetUserId();
            var blogToUpdate = db.Blogs.Find(id);

            if (blogToUpdate.UserID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (selectedTags != null && selectedTags.Length > MAX_BLOG_TAGS)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (TryUpdateModel(blogToUpdate, "",
               new string[] { "Title", "Content" }))
            {
                try
                {

                    UpdateBlogTags(selectedTags, blogToUpdate);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(blogToUpdate);
        }

        private void UpdateBlogTags(string[] selectedTags, Blog blogToUpdate)
        {

            if (selectedTags == null)
            {
                blogToUpdate.Tags.Clear();
                return;
            }

            // Get selected tags and all tags from DB
            var selectedTagsHS = new HashSet<string>(selectedTags);
            var blogTags = new HashSet<int>
                (blogToUpdate.Tags.Select(c => c.TagID));
                
            // Loop through DB tags
            foreach (var tag in db.Tags)
            {
                // If tag is one of the selected ones, add it, else remove it
                if (selectedTagsHS.Contains(tag.TagID.ToString()))
                {
                    if (!blogTags.Contains(tag.TagID))
                    {
                        blogToUpdate.Tags.Add(tag);
                    }
                }
                else
                {
                    if (blogTags.Contains(tag.TagID))
                    {
                        blogToUpdate.Tags.Remove(tag);
                    }
                }
            }
        }


        // POST: MyBlog/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userID = User.Identity.GetUserId();
            Blog blog = await db.Blogs.FindAsync(id);

            if(blog.UserID == userID)
            {
                db.Blogs.Remove(blog);
                await db.SaveChangesAsync();

                return Json(new { success = true, blogId = id });
            }

            return Json(new { success = false });

        }


        public ActionResult BlogTagsJSON()
        {

           var tags = db.Tags.Select(c => new { c.TagID, c.Name});

            return Json(tags, JsonRequestBehavior.AllowGet);
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
