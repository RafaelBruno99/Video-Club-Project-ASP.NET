using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoClub_Final.Data;
using VideoClub_Final.Data.Migrations;
using VideoClub_Final.Models;
using VideoClub_Final.Models.ViewModel;
using VideoClub_Final.Utility;

namespace VideoClub_Final.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ReservationsController : Controller
    {


        private readonly ApplicationDbContext _db;

        public ReservationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string searchName = null, string searchEmail = null,
            string searchPhoneNumber = null, string searchReservationDate = null)
        {

            var currentUser = this.User;
            var claimsIdentity = currentUser;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ReservationViewModel reservationViewModel = new ReservationViewModel()
            {
                Reservations = new List<Models.Reservation>()
            };



            reservationViewModel.Reservations = _db.Reservation.Include(a => a.StoreUser).ToList();
            if (User.IsInRole(SD.AdminEndUser))
            {
                reservationViewModel.Reservations =
                    reservationViewModel.Reservations.Where(a => a.StoreId == claim.Value).ToList();
            }

            if (searchName != null)
            {
                reservationViewModel.Reservations = reservationViewModel.Reservations
                    .Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();
            }

            if (searchEmail != null)
            {
                reservationViewModel.Reservations = reservationViewModel.Reservations
                    .Where(a => a.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).ToList();
            }

            if (searchPhoneNumber != null)
            {
                reservationViewModel.Reservations = reservationViewModel.Reservations
                    .Where(a => a.CustomerPhoneNumber.ToLower().Contains(searchPhoneNumber.ToLower())).ToList();
            }

            if (searchReservationDate != null)
            {
                try
                {
                    DateTime appDate = Convert.ToDateTime(searchReservationDate);
                    reservationViewModel.Reservations = reservationViewModel.Reservations
                        .Where(a => a.ReservationDate.ToShortDateString().Equals(appDate.ToShortDateString())).ToList();
                }
                catch (Exception e)
                {
                }

            }

            return View(reservationViewModel);
        }
        
        //Get Edit

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.ProductReserved
                                                          on p.Id equals a.ProductId
                                                      where a.ReservationId == id
                                                      select p).Include("ProductTypes");

            ReservationDetailsViewModel objReservationViewModel = new ReservationDetailsViewModel()
            {
                Reservation = _db.Reservation.Include(a => a.StoreUser).Where(a => a.Id == id).FirstOrDefault(),
                StoreUser = _db.ApplicationUser.ToList(),
                Products = productList.ToList()
            };

            return View(objReservationViewModel);
        }

        //Post Edit+

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, ReservationDetailsViewModel ojbDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                ojbDetailsViewModel.Reservation.ReservationDate = ojbDetailsViewModel.Reservation.ReservationDate
                    .AddHours(ojbDetailsViewModel.Reservation.ReservationTime.Hour)
                    .AddHours(ojbDetailsViewModel.Reservation.ReservationTime.Minute);

                var reservationFromDB = _db.Reservation.Where(a => a.Id == ojbDetailsViewModel.Reservation.Id)
                    .FirstOrDefault();

                reservationFromDB.CustomerName = ojbDetailsViewModel.Reservation.CustomerName;
                reservationFromDB.CustomerEmail = ojbDetailsViewModel.Reservation.CustomerEmail;
                reservationFromDB.CustomerPhoneNumber = ojbDetailsViewModel.Reservation.CustomerPhoneNumber;
                reservationFromDB.ReservationDate = ojbDetailsViewModel.Reservation.ReservationDate;
                reservationFromDB.isConfirmed = ojbDetailsViewModel.Reservation.isConfirmed;

                if (User.IsInRole(SD.SuperAdminEndUser))
                {
                    reservationFromDB.StoreId = ojbDetailsViewModel.Reservation.StoreId;
                }

                _db.SaveChanges();

            }

            return View(ojbDetailsViewModel);

        }

        //Get Details

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                join a in _db.ProductReserved
                    on p.Id equals a.ProductId
                where a.ReservationId == id
                select p).Include("ProductTypes");

            ReservationDetailsViewModel objReservationViewModel = new ReservationDetailsViewModel()
            {
                Reservation = _db.Reservation.Include(a => a.StoreUser).Where(a => a.Id == id).FirstOrDefault(),
                StoreUser = _db.ApplicationUser.ToList(),
                Products = productList.ToList()
            };

            return View(objReservationViewModel);
        }

        //Delete Action Method

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                join a in _db.ProductReserved
                    on p.Id equals a.ProductId
                where a.ReservationId == id
                select p).Include("ProductTypes");

            ReservationDetailsViewModel objReservationViewModel = new ReservationDetailsViewModel()
            {
                Reservation = _db.Reservation.Include(a => a.StoreUser).Where(a => a.Id == id).FirstOrDefault(),
                StoreUser = _db.ApplicationUser.ToList(),
                Products = productList.ToList()
            };

            return View(objReservationViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _db.Reservation.FindAsync(id);
            _db.Reservation.Remove(reservation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}