using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }
        [Display(Name = "Pickup day (required)")]
        public string TrashPickUpDay { get; set; }
        [Display(Name = "One-time pickup date (optional) (i.e. MM/DD/YYYY)")]
        public int ExtraPickUpRequest { get; set; }
        [Display(Name = "Due balance on account")]
        public double AccountBalance { get; set; }
        [Display(Name = "Pickup suspension start date (optional)")]
        public string StartPickUpSuspension { get; set; }
        [Display(Name = "Pickup suspension end date (optional)")]
        public string EndPickUpSuspension { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

    }
}
