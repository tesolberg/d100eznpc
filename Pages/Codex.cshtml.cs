using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using D100EZNPC.Models;
using D100EZNPC.Services;

namespace D100EZNPC.Pages
{
    public class CodexModel : PageModel
    {
        internal List<NPC>? NPCs;

        readonly INPCService service;

		[ActivatorUtilitiesConstructor]
		public CodexModel(JsonFileNPCService JsonFileNPCService)
        {
            service = JsonFileNPCService;
        }

        public void OnGet()
        {
            NPCs = (List<NPC>?)service.GetAllNPCs();

            NPCs = NPCs?.OrderBy(x => x.Name).ToList();

        }

        public IActionResult OnPostDelete(int id)
        {
            service.DeleteNPC(id);

            return RedirectToAction("Get");
        }
    }
}
