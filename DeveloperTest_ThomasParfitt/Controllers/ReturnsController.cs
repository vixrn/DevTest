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
    public class ReturnsController : Controller
    {
        private static int baseDayRental = 400;
        private static int kmPrice = 30;
        private static int price;
        private CarRentalDBModel db = new CarRentalDBModel();

        // GET: Returns
        public ActionResult Index()
        {
            var returns = db.Returns.Include(r => r.Reservations);
            return View(returns.ToList());
        }

        // GET: Returns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        // GET: Returns/Create
        public ActionResult Create()
        {
            ViewBag.DateTime = DateTime.Now;
            ViewBag.BookingNumber = new SelectList(db.Reservations, "Id", "BookingNr");            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookingNumber,ReturnDate,ReturnCarMilage")] Returns returns)
        {
            if (ModelState.IsValid)
            {
                db.Returns.Add(returns);
                
                Reservations res = db.Reservations.Find(returns.BookingNumber); //Använd booking nr för att hämta info om reservation
                Vehicle vehicle = db.Vehicle.FirstOrDefault(x => x.Id == res.VehID); //Hämta bilen med hjälp av reservationens vehid

                int totalMilage = returns.ReturnCarMilage - res.CarMilage; //Får reda på total milage
                vehicle.CurrentMilage = returns.ReturnCarMilage; //Uppdatera fordonets ny milage
                int days = ((TimeSpan)(returns.ReturnDate- res.RentalDate)).Days; //Behöver antal dagar. Om det lämnas samma dag blir det mindre än 1
                if (days < 1) //Fixa priset
                {
                    days = 1;
                }
                if (vehicle.VehicleType == "Small car")
                {
                    price = baseDayRental * days;
                }
                else if(vehicle.VehicleType == "Van"){
                    double val = baseDayRental * days * 1.2 + kmPrice * totalMilage;
                    price = Convert.ToInt32(val);
                }
                else
                {
                    double val = baseDayRental * days * 1.7 + (kmPrice * totalMilage * 1.5);
                    price = Convert.ToInt32(val);
                }
                //Uppdatera db med de ändringar
                res.price = price;
                db.Entry(res).State = EntityState.Modified;
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();                
                return RedirectToAction("Index");
            }
            ViewBag.BookingNumber = new SelectList(db.Reservations, "Id", "BookingNr", returns.BookingNumber);
            return View(returns);
        }

        // GET: Returns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookingNumber = new SelectList(db.Reservations, "Id", "Id", returns.BookingNumber);
            return View(returns);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookingNumber,ReturnDate,ReturnCarMilage")] Returns returns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(returns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookingNumber = new SelectList(db.Reservations, "Id", "Id", returns.BookingNumber);
            return View(returns);
        }

        // GET: Returns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Returns returns = db.Returns.Find(id);
            if (returns == null)
            {
                return HttpNotFound();
            }
            return View(returns);
        }

        // POST: Returns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Returns returns = db.Returns.Find(id);
            db.Returns.Remove(returns);
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
