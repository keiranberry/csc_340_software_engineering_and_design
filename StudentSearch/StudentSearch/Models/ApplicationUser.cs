using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentSearch.Models
{
    public class ApplicationUser : IdentityUser
    {
        [RegularExpression(@"^[\w\s]*$",
                ErrorMessage = "Must contain only valid characters A-Z and Numbers"), StringLength(30)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [RegularExpression(@"^[0-9]+[\.a-zA-Z\s]*$",
                ErrorMessage = "Must be a valid US Street Address eg. 123 Park Dr."), StringLength(30)]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$",
                ErrorMessage = "Must be a valid US city"), StringLength(30)]
        [Display(Name = "City")]
        public string City { get; set; }

        [RegularExpression(@"^[A-Z]*$",
                ErrorMessage = "Must be a valid 2-letter US state code. eg. NY, SD, CA"), StringLength(2)]
        [Display(Name = "State")]
        public string State { get; set; }

        [RegularExpression(@"^[0-9]{5,5}$",
                ErrorMessage = "Must be a 5 digit US zip code"), StringLength(5)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

    }
}
