using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoClub_Final.Models
{
    public class ProductReserved
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }


        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }  

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }
    }
}
