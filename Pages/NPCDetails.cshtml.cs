using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace D100EZNPC.Pages
{
	public class NPCDetailsModel : PageModel
	{
		private readonly JsonFileNPCService service;
		private readonly ILogger<NPCDetailsModel> _logger;

		public NPC npc { get; set; } = default!;

        public NPCDetailsModel(JsonFileNPCService _service, ILogger<NPCDetailsModel> logger)
		{
			service = _service;
			_logger = logger;
		}

		public void OnGet(string id)
		{
			npc = service.GetNPC(int.Parse(id));
		}

		public void OnPost()
		{
			_logger.LogInformation("Generic ajax post recieved");
		}


		public ActionResult OnPostCycleSkillLevel(int id, string skillName)
		{
			npc = service.GetNPC(id);

			Skill skill = npc.GetSkill(skillName);

			if (skill == null) return BadRequest("Skill not found");

			skill.Value = (skill.Value + 1) % 7;

			service.EditNPC(npc); ;
			_logger.LogInformation(skillName + ": " + skill.Value.ToString());

			return Content(skill.Value.ToString());
		}


		public IActionResult OnPostSetUnique(int id, bool unique)
		{
			NPC updatedNpc = service.GetNPC(id);

			_logger.LogInformation("Setting " + updatedNpc.Name + " unique to " + unique);
		
			updatedNpc.Unique = unique;

			service.EditNPC(updatedNpc);

			return RedirectToAction("Get", new { id });
		}
	}
}
