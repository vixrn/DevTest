using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeveloperTest_ThomasParfitt.Models;

namespace DeveloperTest_ThomasParfitt.Controllers
{
    public class ReservationsController : Controller
    {
        private CarRentalDBModel db = new CarRentalDBModel();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Vehicle);
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            return View(reservations);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.VehID = new SelectList(db.Vehicle, "Id", "VehicleType");
            ViewBag.DateTime = DateTime.Now;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookingNr,CustDoB,RentalDate,VehID,CarMilage")] Reservations reservations)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VehID = new SelectList(db.Vehicle, "Id", "VehicleType", reservations.VehID);
            return View(reservations);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehID = new SelectList(db.Vehicle, "Id", "VehicleType", reservations.VehID);
            return View(reservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookingNr,CustDoB,RentalDate,VehID,CarMilage")] Reservations reservations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VehID = new SelectList(db.Vehicle, "Id", "VehicleType", reservations.VehID);
            return View(reservations);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservations reservations = db.Reservations.Find(id);
            if (reservations == null)
            {
                return HttpNotFound();
            }
            return View(reservations);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservations reservations = db.Reservations.Find(id);
            db.Reservations.Remove(reservations);
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
