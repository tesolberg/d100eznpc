using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D100EZNPC.Pages
{
    public class ModelBindingModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost(string name, string email)
        {
            ViewData["confirmation"] = $"{name}, information will be sent to {email}";
        }
    }
}
