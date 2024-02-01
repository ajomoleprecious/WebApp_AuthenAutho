using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "MustBeHRManager")]
    public class HRmanagerModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
