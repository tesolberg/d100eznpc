namespace D100EZNPC.Models
{
    public class Skill
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public SkillType Type { get; set; }
        public string Description { get; set; }

        public Skill(string name, int value, SkillType type, string description = "")
        { 
            Name = name;
            Value = value;
            Type = type;
            Description = description;
        }
    }

    public enum SkillType { STANDARD, PROFESSIONAL}
}
