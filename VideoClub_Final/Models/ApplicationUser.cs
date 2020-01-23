using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace VideoClub_Final.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Display(Name = "Store")]
        public string Name { get; set; }

        [NotMapped]
        public bool isSuperAdmin { get; set; }
    }
}
