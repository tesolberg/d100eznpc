using D100EZNPC.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace D100EZNPC.Data
{
	public static class ModelBuilderExtensions
	{
		public static ModelBuilder Seed(this ModelBuilder modelBuilder)
		{
			NPC npc1 = new NPC("Celt");
			npc1.Id = -1;
			npc1.HitLocations.Id = -1;

			List<Weapon> weapons= new List<Weapon>();
			Weapon shortSword = new Weapon("Short sword");
			shortSword.Id = -1;

			weapons.Add(shortSword);
			npc1.Weapons = weapons;

			modelBuilder.Entity<NPC>().HasData(
				npc1
				);
			return modelBuilder;
		}
	}
}
