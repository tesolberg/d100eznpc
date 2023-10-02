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

		public List<List<Skill>>? SkillLists { get; set; }

		public NPCDetailsModel(JsonFileNPCService _service, ILogger<NPCDetailsModel> logger)
		{
			service = _service;
			_logger = logger;
		}

		public void OnGet(string id)
		{
			npc = service.GetNPC(int.Parse(id));
			SortSkills();
		}


		public IActionResult OnPost(int id, string skillName)
		{
			CycleSkillLevel(skillName, id);

			return RedirectToAction("Get", new { id });
		}

		private void CycleSkillLevel(string skillName, int id)
		{
			_logger.Log(LogLevel.Information, "Cycle skill");

			npc = service.GetNPC(id);

			Skill skill = npc.Skills.FirstOrDefault(s => s.Name == skillName)!;
			int currentSkillLevel = skill is not null ? skill.Value : 0;
			int newSkillLevel = 0;

			if (currentSkillLevel == npc.BaseCompetence) newSkillLevel = npc.SecondaryCompetence;
			else if (currentSkillLevel == npc.SecondaryCompetence) newSkillLevel = npc.PrimaryCompetence;
			else newSkillLevel = npc.BaseCompetence;

			skill!.Value = newSkillLevel;

			service.EditNPC(npc);
		}

		private void SortSkills()
		{
			SkillLists = new List<List<Skill>>();

			foreach (var skill in npc.Skills)
			{
				bool found = false;

				foreach (var list in SkillLists)
				{
					if (list.First<Skill>().Value.Equals(skill.Value))
					{
						list.Add(skill);
						found = true;
						break;
					}
				}

				if (!found) SkillLists.Add(new List<Skill> { skill });
			}

			foreach (var list in SkillLists)
			{
			}
		}
	}
}
