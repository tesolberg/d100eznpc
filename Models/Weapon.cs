using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D100EZNPC.Models
{
	public class Weapon
	{
        // Basics
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string Name { get; set; }
        public bool Unique { get; set; }
        public string Notes { get; set; }

        // Stats
        public string Damage { get; set; }
        public Size Size { get; set; }
        public Reach Reach { get; set; }
        public string CombatEffects { get; set; }
        public int Encumberance { get; set; }
        public int AP { get; set; }
        public int HP { get; set; }
        public string Traits { get; set; }
        public int Cost { get; set; }

        public Weapon(string name, string damage = "1d6", bool unique = false)
        {
            Name = name; 
            Unique = unique;
            Notes = string.Empty;
            Damage = damage;
            Size = Size.MEDIUM;
            Reach = Reach.MEDIUM;
            CombatEffects = string.Empty;
            Encumberance = 0;
            AP = 0;
            HP = 0;
            Traits = string.Empty;
            Cost = 0;
        }

    }

    public enum Size { SMALL, MEDIUM, LARGE, HUGE, ENORMOUS }
    public enum Reach { TOUCH, SHORT, MEDIUM, LONG, VERYLONG}
}
