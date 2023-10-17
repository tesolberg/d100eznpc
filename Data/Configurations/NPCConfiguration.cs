using D100EZNPC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D100EZNPC.Data.Configurations
{
	public class NPCConfiguration : IEntityTypeConfiguration<NPC>
	{
		public void Configure(EntityTypeBuilder<NPC> builder)
		{
		}
	}
}
