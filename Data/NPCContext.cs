using D100EZNPC.Data.Configurations;
using D100EZNPC.Models;
using Microsoft.EntityFrameworkCore;

namespace D100EZNPC.Data
{
	public class NPCContext : DbContext
	{
		public DbSet<NPC> NPCs { get; set; }
		public DbSet<Weapon> Weapons { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlite(@"Data source=NPC.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new NPCConfiguration());
		}
	}
}
