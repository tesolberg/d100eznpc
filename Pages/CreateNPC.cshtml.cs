using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D100EZNPC.Pages
{

    public class CreateNPCModel : PageModel
    {
        [BindProperty]
        public string newNpcName { get; set; } = default!;

        readonly JsonFileNPCService service;

        [ActivatorUtilitiesConstructor]
        public CreateNPCModel(JsonFileNPCService JsonFileNPCService)
        {
            service = JsonFileNPCService;
        }


        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string name = Request.Form["Name"]!;
            if (name != "")
            {
                NPC newNPC = new NPC(0, name);
                service.AddNewNPC(newNPC);
            }

            return RedirectToPage("Codex");
        }
    }
}
