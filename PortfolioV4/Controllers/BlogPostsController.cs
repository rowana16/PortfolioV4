﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortfolioV4.Models;
using PortfolioV4.Models.codeFirst;
using System.IO;
using PagedList;
using PagedList.Mvc;


namespace PortfolioV4.Controllers
{
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index(string searchStr, int? page)
        {
            var result = db.Posts.Where(p => p.Body.Contains(searchStr))
                    .Union(db.Posts.Where(p => p.Title.Contains(searchStr)))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.Body.Contains(searchStr))))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.DisplayName.Contains(searchStr))))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.FirstName.Contains(searchStr))))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.LastName.Contains(searchStr))))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.UserName.Contains(searchStr))))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.Email.Contains(searchStr))))
                    .Union(db.Posts.Where(p => p.Comments.Any(c => c.UpdateReason.Contains(searchStr))));

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (searchStr != null)
            {
                if(result == null)
                {
                    ViewBag.Alert = "No Results Found";
                    return View();
                }

                else
                {
                    return View(result.OrderByDescending(i => i.Created).ToPagedList(pageNumber, pageSize));
                }
            }
            
            return View(db.Posts.OrderByDescending(i=>i.Created).ToPagedList(pageNumber,pageSize));
        }

       
//=============================================== Details ============================================================
        // GET: BlogPosts/Details/5
        //public ActionResult Details(int id)
        //{
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}

        //    BlogPost blogPost = db.Posts.Find(id);
        //    if (blogPost == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(blogPost);
        //}

        public ActionResult Details(string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           

            BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == Slug );
            if (blogPost == null)
            {
                return HttpNotFound();
            }



            return View(blogPost);
        }
        //=============================================== Start Create ============================================================
        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                //check the file name to make sure its an image
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".gif" && ext != ".jpeg" && ext != ".bmp") { ModelState.AddModelError("image", "Invalid Format."); }
            }
            
            if (ModelState.IsValid)
            {
                blogPost.Created = DateTime.Now;
                blogPost.Updated = DateTime.Now;
                if (image != null)
                {
                    //relative server path  // Ensure the folder gets published for the images to work.
                    var filePath = "/Uploads/";
                    //path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);
                    //Medial url for relative path
                    blogPost.MediaURL = filePath + image.FileName;
                    //Save Image to Local Variable
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }

                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title.");
                    return View(blogPost);
                }
                
                if (db.Posts.Any(p=>p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique.");
                    return View(blogPost);
                }

                blogPost.Slug = Slug;

                db.Posts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        
//=============================================== Start Edit ============================================================
        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created, Updated,Title,Slug,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                //check the file name to make sure its an image
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".gif" && ext != ".bmp" && ext != ".jpeg") { ModelState.AddModelError("image", "Invalid Format."); }
            }

            if (ModelState.IsValid)
            {
                blogPost.Updated = DateTime.Now;
                if (image != null)
                {
                    //relative server path
                    var filePath = "/Uploads/";
                    //path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);
                    //Medial url for relative path
                    blogPost.MediaURL = filePath + image.FileName;
                    //Save Image to Local Variable
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }

                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid Title.");
                    return View(blogPost);
                }

                blogPost.Slug = Slug;

                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

       
        //=============================================== Start Delete ============================================================
        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
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
