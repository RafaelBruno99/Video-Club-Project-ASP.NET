using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoClub_Final.Data;
using VideoClub_Final.Models;
using VideoClub_Final.Models.ViewModel;
using VideoClub_Final.Extensions;
using Microsoft.EntityFrameworkCore;

namespace VideoClub_Final.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()
            }
            ;
        }

        //Get Index Shopping Cart
        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0)
            {
                foreach (int cartItem in lstShoppingCart)
                {
                    Products prod = _db.Products.Include(P=>P.ProductTypes).Where(p => p.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.Products.Add(prod);
                }
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            ShoppingCartVM.Reservation.ReservationDate = ShoppingCartVM.Reservation.ReservationDate
                .AddHours(ShoppingCartVM.Reservation.ReservationTime.Hour)
                .AddMinutes(ShoppingCartVM.Reservation.ReservationTime.Minute);

            Reservation reservation = ShoppingCartVM.Reservation;
            _db.Reservation.Add(reservation);
            _db.SaveChanges();

            int reservationId = reservation.Id;

            foreach (int productId in lstCartItems )
            {
                ProductReserved productReserved = new ProductReserved()
                {
                    ReservationId = reservationId,
                    ProductId = productId

                };

                _db.ProductReserved.Add(productReserved);
                
            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction("ReservationConfirmation", "ShoppingCart", new{Id= reservationId});
        }

        public IActionResult Remove(int id)
        {

            //Remove products form Shopping Cart

            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(id))
                {
                    lstCartItems.Remove(id);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }

        //Get Action Method for the Reservation Confirmation
        public IActionResult ReservationConfirmation(int id)
        {
            ShoppingCartVM.Reservation = _db.Reservation.Where(r => r.Id == id).FirstOrDefault();
            List<ProductReserved> ProdList = _db.ProductReserved.Where(p => p.ReservationId == id).ToList();

            foreach (ProductReserved prodAptObj in ProdList)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.ProductTypes)
                    .Where(p => p.Id == prodAptObj.ProductId).FirstOrDefault());
            }

            return View(ShoppingCartVM);
        }
    }
}