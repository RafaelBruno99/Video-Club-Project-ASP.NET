using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoClub_Final.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Display(Name="Store")]
        public string StoreId { get; set; }

        [ForeignKey("StoreId")]
        public virtual ApplicationUser StoreUser { get; set; }

        public DateTime ReservationDate { get; set; }

        [NotMapped]
        public DateTime ReservationTime { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public string CustomerEmail { get; set; }

        public bool isConfirmed { get; set; }

    }
}
