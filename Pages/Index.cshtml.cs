using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace D100EZNPC.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly INPCService service;

        public List<NPC>? NPCs { get; set; }

        public IndexModel(ILogger<IndexModel> logger, JsonFileNPCService _service)
		{
			_logger = logger;
			service = _service;
		}


		public IActionResult OnGet()
		{
			UpdateNPCList();

			return Page();
		}

		public void OnPost()
		{
			_logger.LogInformation("On Post recieved on Index");
		}

		public ActionResult OnPostAddToTracker(int id)
		{
			_logger.LogInformation($"Adding NPC {id} to tracker");
			return Content(service.GetNPC(id).ToString());
		}

		private void UpdateNPCList()
		{
			NPCs = (List<NPC>)service.GetAllNPCs()!;
			NPCs = NPCs?.OrderBy(x => x.Name).ToList()!;
		}
	}

}