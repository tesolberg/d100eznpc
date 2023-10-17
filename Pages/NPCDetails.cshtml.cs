using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

namespace D100EZNPC.Pages
{
	public class NPCDetailsModel : PageModel
	{
		private readonly JsonFileNPCService service;
		private readonly ILogger<NPCDetailsModel> _logger;

		[BindProperty]
		public NPC npc { get; set; } = default!;
		[BindProperty]
		public int ScrollPosition { get; set; }

		public NPCDetailsModel(JsonFileNPCService _service, ILogger<NPCDetailsModel> logger)
		{
			service = _service;
			_logger = logger;
		}

		public IActionResult OnGet(string id)
		{
			if (TempData.ContainsKey("ScrollPosition"))
			{
				if (TempData.Peek("ScrollPosition") is int scrollPosition)
				{
					ScrollPosition = scrollPosition;
				}
			}

			npc = service.GetNPC(int.Parse(id));
		
			return Page();
		}

		public void OnPost()
		{
			_logger.LogInformation("Ajax post recieved");
		}


		public IActionResult OnPostCycleSkillLevel(int id, string skillName)
		{
			TempData["ScrollPosition"] = ScrollPosition;

			npc = service.GetNPC(id);

			if (npc.BaseSkills.Contains(skillName))
			{
				npc.BaseSkills.Remove(skillName);
				npc.SecondarySkills.Add(skillName);
			}
			else if (npc.SecondarySkills.Contains(skillName))
			{
				npc.SecondarySkills.Remove(skillName);
				npc.PrimarySkills.Add(skillName);
			}
			else if (npc.PrimarySkills.Contains(skillName))
			{
				npc.PrimarySkills.Remove(skillName);
				npc.BaseSkills.Add(skillName);
			}
			else _logger.LogWarning($"{skillName} was not found in any of npc \"{npc.Name}\"'s skill lists");

			service.EditNPC(npc); ;

			return RedirectToAction("Get", new { id });
		}

		public IActionResult OnPostUpdateSkillLevels(int id, int baseCompetence)
		{
			NPC updatedNpc = service.GetNPC(id);
			updatedNpc.BaseCompetence = npc.BaseCompetence;
			updatedNpc.SecondaryCompetence = npc.SecondaryCompetence;
			updatedNpc.PrimaryCompetence = npc.PrimaryCompetence;

			service.EditNPC(updatedNpc);

			return RedirectToAction("Get", new { id });
		}

		public IActionResult OnPostToggleUnique(int id)
		{
			_logger.LogInformation("Toggleing generic for id " + id.ToString());

			TempData["ScrollPosition"] = ScrollPosition;

			NPC updatedNpc = service.GetNPC(id);

			updatedNpc.Unique = !updatedNpc.Unique;

			service.EditNPC(updatedNpc);

			return RedirectToAction("Get", new { id });
		}


		// PRIVATE METHODS

		private void CycleSkillLevel(string skillName, int id)
		{

		}

	}
}
