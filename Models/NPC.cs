using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace D100EZNPC.Models
{
	public class NPC
	{

		public int Id { get; set; } = 0;

		[Required]
		public string Name { get; set; } = default!;
		public int TokenNumber { get; set; } = default;
		public bool Unique { get; set; } = false;

		public string Notes { get; set; } = default!;

		// Skills
		public IList<String> PrimarySkills { get; set; } = default!;
		public IList<String> SecondarySkills { get; set; } = default!;
		public IList<String> BaseSkills { get; set; } = default!;

		public int PrimaryCompetence { get; set; }
		public int SecondaryCompetence { get; set; }
		public int BaseCompetence { get; set; }

		// Weapons
		public IList<Weapon> Weapons { get; set; }

        // Hit locations
        public IList<HitLocation> HitLocations { get; set; } = new List<HitLocation>();

		public NPC()
		{

		}

		public NPC(int id, string name,  int primaryCompetence = 75, int secondaryCompetence = 45, int baseCompetence = 20, bool unique = false)
		{
			Id = id;
			Name = name;
			TokenNumber = 0;
			Unique = unique;
			Notes = "";
			PrimaryCompetence = primaryCompetence;
			SecondaryCompetence = secondaryCompetence;
			BaseCompetence = baseCompetence;

			Weapons = new List<Weapon>();

			GenerateSkills();
			GenerateHitLocations();
		}

		// Initalizes all skills to base level
		public void GenerateSkills()
		{
			PrimarySkills = new List<String>();
			SecondarySkills = new List<String>();
			BaseSkills = new List<String>();

			for (int i = 0; i < SkillLibrary.standardSkills.Length; i++)
			{
				BaseSkills.Add(SkillLibrary.standardSkills[i]);
			}

			for (int i = 0; i < SkillLibrary.professionalSkills.Length; i++)
			{
				BaseSkills.Add(SkillLibrary.professionalSkills[i]);
			}
		}

		// Hit Locations
		public void GenerateHitLocations(int conPlusSize = 16)
		{
			HitLocations.Add(new HitLocation("Right Leg", 1, 3, 0, conPlusSize));
			HitLocations.Add(new HitLocation("Left Leg", 4, 6, 0, conPlusSize));
			HitLocations.Add(new HitLocation("Abdomen", 7, 9, 1, conPlusSize));
			HitLocations.Add(new HitLocation("Chest", 10, 12, 2, conPlusSize));
			HitLocations.Add(new HitLocation("Right Arm", 13, 15, -1, conPlusSize));
			HitLocations.Add(new HitLocation("Left Arm", 16, 18, -1, conPlusSize));
			HitLocations.Add(new HitLocation("Head", 19, 20, 0, conPlusSize));
		}

		public override string ToString() => JsonSerializer.Serialize<NPC>(this);
		public string ToStringFormatted() => JsonSerializer.Serialize<NPC>(this, new JsonSerializerOptions { WriteIndented = true });
	}


	public class HitLocation
	{
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
}