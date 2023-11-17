using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace D100EZNPC.Models
{
	public class NPC
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = default!;
		public int TokenNumber { get; set; } = default;
		public bool Unique { get; set; } = false;
        public int HitPoints { get; set; }
        public string Notes { get; set; } = default!;

		// Skills
		public List<Skill> Skills { get; set; } = default!;


		// Weapons
		public List<Weapon>? Weapons { get; set; }

		public NPC()
		{

		}

		public NPC(string name, bool unique = false, List<Weapon> weapons = default!)
		{
			Name = name;
			TokenNumber = 0;
			Unique = unique;
			Notes = "";

			Weapons = weapons is null ? new List<Weapon>() : weapons;

			GenerateSkills();
		}

		// Initalizes all skills to base level
		public void GenerateSkills()
		{
			Skills = new List<Skill>();

			for (int i = 0; i < SkillLibrary.standardSkills.Length; i++)
			{
				Skills.Add(new Skill(SkillLibrary.standardSkills[i], 0));
			}

			for (int i = 0; i < SkillLibrary.professionalSkills.Length; i++)
			{
				Skills.Add(new Skill(SkillLibrary.professionalSkills[i], 0));
			}
		}

		public Skill GetSkill(string skillName)
		{
			foreach (var skill in Skills)
			{
				if (skill.Name == skillName)
				{
					return skill;
				}
			}
			throw new Exception(skillName + " not found");
		}

		public override string ToString() => JsonSerializer.Serialize<NPC>(this);
		public string ToStringFormatted() => JsonSerializer.Serialize<NPC>(this, new JsonSerializerOptions { WriteIndented = true });
	}

	public class Skill
	{
		public string? Name { get; set; }
		[Range(0,6)]
		public int Value { get; set; }
		public string? Notes { get; set; }

		public Skill(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}