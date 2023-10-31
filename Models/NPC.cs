using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace D100EZNPC.Models
{
	public class NPC
	{
		private int primaryCompetence;
		private int secondaryCompetence;
		private int baseCompetence;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = default!;
		public int TokenNumber { get; set; } = default;
		public bool Unique { get; set; } = false;

		public string Notes { get; set; } = default!;

		// Skills
		public List<Skill> Skills { get; set; } = default!;

		public int PrimaryCompetence
		{
			get => primaryCompetence;
			set
			{
				if (Skills != null)
				{
					foreach (Skill skill in Skills)
					{
						if (skill.Value == PrimaryCompetence) skill.Value = value;
					}
				}
				primaryCompetence = value;
			}
		}
		public int SecondaryCompetence
		{
			get => secondaryCompetence; set
			{
				if (Skills != null)
				{
					foreach (Skill skill in Skills)
					{
						if (skill.Value == SecondaryCompetence) skill.Value = value;
					}
				}
				secondaryCompetence = value;
			}
		}
		public int BaseCompetence
		{
			get => baseCompetence; set
			{
				if (Skills != null)
				{
					foreach (Skill skill in Skills)
					{
						if (skill.Value == BaseCompetence) skill.Value = value;
					}
				}
				baseCompetence = value;
			}
		}

		// Weapons
		public List<Weapon>? Weapons { get; set; }

		// Hit locations
		public HitLocations? HitLocations { get; set; }

		public NPC()
		{

		}

		public NPC(string name, int primaryCompetence = 75, int secondaryCompetence = 45, int baseCompetence = 20, bool unique = false, List<Weapon> weapons = default!)
		{
			Name = name;
			TokenNumber = 0;
			Unique = unique;
			Notes = "";
			this.primaryCompetence = primaryCompetence;
			this.secondaryCompetence = secondaryCompetence;
			this.baseCompetence = baseCompetence;

			Weapons = weapons is null ? new List<Weapon>() : weapons;

			GenerateSkills();
			HitLocations = new HitLocations();
		}

		// Initalizes all skills to base level
		public void GenerateSkills()
		{
			Skills = new List<Skill>();

			for (int i = 0; i < SkillLibrary.standardSkills.Length; i++)
			{
				Skills.Add(new Skill(SkillLibrary.standardSkills[i], BaseCompetence));
			}

			for (int i = 0; i < SkillLibrary.professionalSkills.Length; i++)
			{
				Skills.Add(new Skill(SkillLibrary.professionalSkills[i], BaseCompetence));
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

	public class HitLocations
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public HitLocation RightLeg { get; set; }
		public HitLocation LeftLeg { get; set; }

		public HitLocation Abdomen { get; set; }
		public HitLocation Chest { get; set; }
		public HitLocation RightArm { get; set; }
		public HitLocation LeftArm { get; set; }
		public HitLocation Head { get; set; }

		public HitLocations()
		{
			int conPlusSize = 16;
			RightLeg = (new HitLocation("Right Leg", 1, 3, 0, conPlusSize));
			LeftLeg = (new HitLocation("Left Leg", 4, 6, 0, conPlusSize));
			Abdomen = (new HitLocation("Abdomen", 7, 9, 1, conPlusSize));
			Chest = (new HitLocation("Chest", 10, 12, 2, conPlusSize));
			RightArm = (new HitLocation("Right Arm", 13, 15, -1, conPlusSize));
			LeftArm = (new HitLocation("Left Arm", 16, 18, -1, conPlusSize));
			Head = (new HitLocation("Head", 19, 20, 0, conPlusSize));
		}
	}

	public class HitLocation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int HitLocationId { get; set; }

		public HitLocation(string name, int rangeStart, int rangeEnd, int hPMod, int conPlusSize = 16, int armorPoints = 0)
		{
			Name = name;
			RangeStart = rangeStart;
			RangeEnd = rangeEnd;
			ConPlusSize = conPlusSize;
			HPMod = hPMod;
			ArmorPoints = armorPoints;

			HPCurrent = HPMax;
			PassiveBlocked = false;
			Armor = "";
		}

		public string Name { get; set; }
		public int RangeStart { get; set; }
		public int RangeEnd { get; set; }
		public bool PassiveBlocked { get; set; }
		public string Armor { get; set; }
		public int ArmorPoints { get; set; }
		public int HPCurrent { get; set; }

		// HP Max Calculation
		public int ConPlusSize { get; set; }
		public int HPMod { get; set; }
		public int HPMax => StandardHP() + HPMod;

		private int StandardHP()
		{
			int standardHP = ConPlusSize / 5;
			if (ConPlusSize % 5 != 0) standardHP++;
			return standardHP;
		}
	}

	public class Skill
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Value { get; set; }
		public string? Notes { get; set; }

		public Skill(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}