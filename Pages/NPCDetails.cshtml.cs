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

		[BindProperty]
		public NPC npc { get; set; } = default!;
        [BindProperty]
		public int BaseCompetence { get; set; }
		[BindProperty]
		public int SecondaryCompetence { get; set; }
		[BindProperty]
		public int PrimaryCompetence { get; set; }
		
		public NPCDetailsModel(JsonFileNPCService _service, ILogger<NPCDetailsModel> logger)
		{
			service = _service;
			_logger = logger;
		}

		public IActionResult OnGet(string id)
		{
			npc = service.GetNPC(int.Parse(id));
			BaseCompetence = npc.BaseCompetence;
			SecondaryCompetence = npc.SecondaryCompetence;
			PrimaryCompetence = npc.PrimaryCompetence;

			return Page();
		}


		public IActionResult OnPostCycleSkillLevel(int id, string skillName)
		{
			CycleSkillLevel(skillName, id);

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


		// PRIVATE METHODS

		private void CycleSkillLevel(string skillName, int id)
		{
			_logger.Log(LogLevel.Information, "Cycle skill");

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

            service.EditNPC(npc);
		}

	}
}
