namespace D100EZNPC.Models
{
	public class Weapon
	{
        // Basics
        public string Name { get; set; }
        public bool Unique { get; set; }
        public string Notes { get; set; }

        // Stats
        public string Damage { get; set; }
        public string Size { get; set; }
        public string Reach { get; set; }
        public string CombatEffects { get; set; }
        public int Encumberance { get; set; }
        public int AP { get; set; }
        public int HP { get; set; }
        public int Traits { get; set; }
        public int Cost { get; set; }

    }
}
