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

        public IList<Skill> Skills { get; set; } = default!;

        public int PrimaryCompetence { get; set; } = 75;

        public int SecondaryCompetence { get; set; } = 45;

        public int BaseCompetence { get; set; } = 20;



        public void GenerateSkills()
        {
            Skills = new List<Skill>();

            for (int i = 0; i < SkillLibrary.standardSkills.Length; i++)
            {
                Skills.Add(new Skill(SkillLibrary.standardSkills[i], BaseCompetence, SkillType.STANDARD));
            }

            for (int i = 0; i < SkillLibrary.professionalSkills.Length; i++)
            {
                Skills.Add(new Skill(SkillLibrary.professionalSkills[i], BaseCompetence, SkillType.PROFESSIONAL));
            }   
        }
	
		public override string ToString() => JsonSerializer.Serialize<NPC>(this);
        public string ToStringFormatted() => JsonSerializer.Serialize<NPC>(this, new JsonSerializerOptions { WriteIndented = true });
	}




}