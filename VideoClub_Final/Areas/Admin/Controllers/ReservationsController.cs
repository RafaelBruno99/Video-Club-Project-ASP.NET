using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoClub_Final.Data;
using VideoClub_Final.Models;
using VideoClub_Final.Models.ViewModel;
using VideoClub_Final.Utility;

namespace VideoClub_Final.Areas.Admin.Controllers
{
    public class ReservationsController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ReservationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles=SD.AdminEndUser + ","+SD.SuperAdminEndUser)]
        [Area("Admin")]
        public IActionResult Index()
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity) this.User.Identities;
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

            return View();
        }
    }
}