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

        public string Description { get; set; } = default!;

        public IList<String> PrimarySkills { get; set; } = default!;
        public IList<String> SecondarySkills { get; set; } = default!;
        public IList<String> BaseSkills { get; set; } = default!;


        public int PrimaryCompetence { get; set; } = 75;
		public int SecondaryCompetence { get; set; } = 45;
		public int BaseCompetence { get; set; } = 20;



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
	
		public override string ToString() => JsonSerializer.Serialize<NPC>(this);
        public string ToStringFormatted() => JsonSerializer.Serialize<NPC>(this, new JsonSerializerOptions { WriteIndented = true });
	}




}