using D100EZNPC.Models;
using D100EZNPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
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

		public List<List<Skill>>? SortedSkills;

        public NPCDetailsModel(JsonFileNPCService _service, ILogger<NPCDetailsModel> logger)
		{
			service = _service;
			_logger = logger;
		}

		public void OnGet(string id)
		{
			npc = service.GetNPC(int.Parse(id));
			SortedSkills = SortSkills();
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

			if (skill.Value == npc.BaseCompetence) skill.Value = npc.SecondaryCompetence;
			else if (skill.Value == npc.SecondaryCompetence) skill.Value = npc.PrimaryCompetence;
			else skill.Value = npc.BaseCompetence;

			service.EditNPC(npc); ;

			return Content(skillName + " value changed to " + skill.Value);
		}

		public IActionResult OnPostUpdateCompetenceLevels(int id)
		{
			NPC updatedNpc = service.GetNPC(id);
			updatedNpc.BaseCompetence = npc.BaseCompetence;
			updatedNpc.SecondaryCompetence = npc.SecondaryCompetence;
			updatedNpc.PrimaryCompetence = npc.PrimaryCompetence;

			_logger.LogInformation($"Updating skill levels to: {updatedNpc.BaseCompetence} / {updatedNpc.SecondaryCompetence} / {updatedNpc.PrimaryCompetence}");

			service.EditNPC(updatedNpc);

			return RedirectToAction("Get", new { id });
		}

		public IActionResult OnPostSetUnique(int id, bool unique)
		{
			NPC updatedNpc = service.GetNPC(id);

			_logger.LogInformation("Setting " + updatedNpc.Name + " unique to " + unique);
		
			updatedNpc.Unique = unique;

			service.EditNPC(updatedNpc);

			return RedirectToAction("Get", new { id });
		}


		public List<List<Skill>> SortSkills()
		{
			List<Skill> baseSkills = new List<Skill>();
			List <Skill> secondarySkills = new List<Skill>();
			List <Skill> primarySkills = new List<Skill>();

			foreach (Skill skill in npc.Skills) 
			{ 
				if (skill.Value == npc.BaseCompetence) baseSkills.Add(skill);
				else if (skill.Value == npc.SecondaryCompetence) secondarySkills.Add(skill);
				else if (skill.Value == npc.PrimaryCompetence) primarySkills.Add(skill);
			}

			return new List<List<Skill>>()
			{
				baseSkills, secondarySkills, primarySkills
			};
		}

	}
}
