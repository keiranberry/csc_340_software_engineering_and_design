// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentSearch.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentSearch.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required]
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

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                CompanyName = user.CompanyName,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.CompanyName != user.CompanyName)
            {
                user.CompanyName = Input.CompanyName;
            }

            if (Input.StreetAddress != user.StreetAddress)
            {
                user.StreetAddress = Input.StreetAddress;
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
            }

            if (Input.State != user.State)
            {
                user.State = Input.State;
            }

            if (Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
