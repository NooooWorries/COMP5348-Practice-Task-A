/*
 * Added file
 * */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VideoStore.Services.MessageTypes;
using VideoStore.WebClient.Binders;
using VideoStore.WebClient.Models;
using VideoStore.WebClient.ViewModels;

namespace VideoStore.WebClient.Controllers
{
    public class ReviewController : Controller
    {
        private VideoStoreWebClientContext db = new VideoStoreWebClientContext();
        static int pMediaId;
        // GET: Review
        [HttpPost]
        public ActionResult Index(int MediaId)
        {
            pMediaId = MediaId;
            ViewBag.MediaId = MediaId;
            List<Review> reviewList = new List<Review>();
            reviewList = db.Reviews.ToList();
            int total = 0;
            int count = 0;
            for (int i = 0; i < reviewList.Count; i ++)
            {
                if (reviewList[i].MediaId == pMediaId)
                {
                    total = total + reviewList[i].Rating;
                    count++;
                }
            }
            double average = Math.Round((double)total / (double)count,2);
            ViewBag.AverageRating = average;
            return View(reviewList);          
        }

        // GET: Review/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Review/Create
        public ActionResult Create()
        {

            ViewBag.User = UserInformation.user.Name;
            ViewBag.Location = UserInformation.user.City + ", " + UserInformation.user.Country;
            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MediaId,ReviewDate,Reviewer,ReviewLocation,ReviewTitle,Rating,ReviewContent")] Review review)
        {
            if (ModelState.IsValid)
            {
                if (review.Rating > 5 || review.Rating < 0)
                {
                    Response.Write("<script>alert('Rating should not higher than 5 or lower than 0');</script>");
                }
                else if (review.ReviewTitle == "")
                {
                    Response.Write("<script>alert('Please provide a review title');</script>");
                }
                else
                {
                    review.MediaId = pMediaId;
                    review.ReviewDate = DateTime.Now;
                    review.Reviewer = UserInformation.user.Name;
                    review.ReviewLocation = UserInformation.user.City + ", " + UserInformation.user.Country;
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("ListMedia", "Store");
                }
            }

            return View(review);
        }


        // GET: Review/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MediaId,ReviewDate,Reviewer,ReviewLocation,ReviewTitle,Rating,ReviewContent")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Review/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
