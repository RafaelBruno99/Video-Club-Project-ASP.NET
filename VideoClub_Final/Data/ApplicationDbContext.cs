using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoClub_Final.Models;

namespace VideoClub_Final.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public  DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<Products> Products { get; set; }

        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<ProductReserved> ProductReserved { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}
