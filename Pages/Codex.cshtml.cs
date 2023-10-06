using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using D100EZNPC.Models;
using D100EZNPC.Services;

namespace D100EZNPC.Pages
{
    public class CodexModel : PageModel
    {
        internal List<NPC>? NPCs;

        [BindProperty]
        public NPC newNpc { get; set; } = default!;

        readonly JsonFileNPCService service;

		[ActivatorUtilitiesConstructor]
		public CodexModel(JsonFileNPCService JsonFileNPCService)
        {
            service = JsonFileNPCService;
        }

        public void OnGet()
        {
            NPCs = (List<NPC>?)service.GetNPCs();

        }

        public IActionResult OnPost()
        {
            // under the hood creates an NPC instance and populates it with binded properties

            if (newNpc == null)
            {
                return Page();
            }

            service.AddNPC(newNpc);

            return RedirectToAction("Get");
        }

        public IActionResult OnPostDelete(int id)
        {
            service.DeleteNPC(id);

            return RedirectToAction("Get");
        }
    }
}
