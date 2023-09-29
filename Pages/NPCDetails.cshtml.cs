using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D100EZNPC.Pages
{
    public class NPCDetailsModel : PageModel
    {
        JsonFileNPCService service;
        NPC? npc;

        public NPCDetailsModel(JsonFileNPCService _service)
        {
            service = _service;
        }

        public void OnGet(string id)
        {
            npc = service.GetNPC(int.Parse(id));
        }
    }
}
