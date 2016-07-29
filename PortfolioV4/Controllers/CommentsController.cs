using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortfolioV4.Models;
using PortfolioV4.Models.codeFirst;

namespace PortfolioV4.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.Post);
            return View(comments.ToList());
        }

        //=============================================== Start Details ============================================================
        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //=============================================== Start Create ============================================================
        // GET: Comments/Create
        public ActionResult Create(int? id)
        {
            ViewBag.PostId = id;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostId,AuthorId,Body,Created,Updated,UpdateReason")] Comment comment)
        {
            comment.Created = DateTime.Now;
            comment.Updated = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                string slug = db.Posts.Find(comment.PostId).Slug.ToString();
                return RedirectToAction(slug, "Blog");
            }

            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", comment.AuthorId);
            //ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }


        //=============================================== Start Edit ============================================================
        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
          
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);           
            ViewBag.Slug = db.Posts.Find(comment.PostId).Slug.ToString();
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostId,AuthorId,Body,Created,Updated,UpdateReason")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Updated = DateTime.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                
                string slug = db.Posts.Find(comment.PostId).Slug.ToString();
                return RedirectToAction(slug, "Blog");
            }
            ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", comment.AuthorId);
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }


        //=============================================== Start Delete ============================================================
        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            string slug = db.Posts.Find(comment.PostId).Slug.ToString();
            return RedirectToAction(slug, "Blog");
        }


        //=============================================== Garbage Collection ============================================================
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
