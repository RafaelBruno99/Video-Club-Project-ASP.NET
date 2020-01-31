using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoClub_Final.Models.ViewModel
{
    public class ReservationDetailsViewModel
    {
        public Reservation Reservation { get; set; }

        public List<ApplicationUser> StoreUser { get; set; }

        public List<Products> Products { get; set; }
    }
}
