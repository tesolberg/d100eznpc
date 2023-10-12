using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D100EZNPC.Pages
{

    public class CreateNPCModel : PageModel
    {
        [BindProperty]
        public NPC newNpc { get; set; } = default!;

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
            // under the hood creates an NPC instance and populates it with binded properties

            if (newNpc == null)
            {
                return Page();
            }

            service.AddNPC(newNpc);

            return RedirectToPage("Codex");
        }
    }
}
