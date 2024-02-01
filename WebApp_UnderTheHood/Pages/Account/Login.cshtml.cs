using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        // 1. Add a Credential property to the LoginModel class.
        // 2. Add a BindProperty attribute to the Credential property to enable model binding.
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Verify the credential by checking the username and password.
            if (Credential.Username == "admin" && Credential.Password == "password")
            {
                // creating the security context
                // 1. Create a list of claims.
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, Credential.Username),
                    new Claim(ClaimTypes.Email, Credential.Username + "@example.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", new DateTime(2023, 05, 1).ToString("yyyy-MM-dd")),
                    };

                // create the identity
                // 2. Create a ClaimsIdentity object.

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");

                //create the principal
                // 3. Create a ClaimsPrincipal object.
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    // Set the IsPersistent property to true to create a persistent cookie.
                    IsPersistent = Credential.RememberMe
                };

                // sign in the user
                // 4. Call the SignInAsync method on the HttpContext object.
                await HttpContext.SignInAsync("MyCookieAuth", principal, authProperties);

                //go to return url or home page

                string? returnUrl = HttpContext.Request.Query["returnUrl"];
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToPage("/Index");
            }
            return Page();
        }
    }

    public class Credential
    {
        // string.Empty is the same as "". It ensures that the property is never null.
        // Required attribute ensures that the property is never null.
        // Display attribute is used to change the display name of the property.
        [Required]
        [Display(Name = "Name")]
        public string Username { get; set; } = string.Empty;
        // DataType attribute is used to change the type of the property.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
