using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Database.Configuration;

public sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
	public void Configure(EntityTypeBuilder<Game> builder)
	{
		builder.HasFlakeIdKey();
	}
}