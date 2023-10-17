using D100EZNPC.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace D100EZNPC.Data
{
	public static class ModelBuilderExtensions
	{
		public static ModelBuilder Seed(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NPC>().HasData(
				new NPC(0, "Celt"),
				new NPC(1, "Roman Legionnaire")
				);
			return modelBuilder;
		}
	}
}
