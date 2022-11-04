using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Detector;

namespace SightKeeper.DAL.Members.Common;

public sealed class Profile
{
	public Guid Id { get; }
	public string Name { get; set; }
	public string Description { get; set; }
	public Game Game { get; set; }
	public DetectorModel DetectorModel { get; set; }


	public Profile(string name, string description, Game game, DetectorModel detectorModel)
	{
		Name = name;
		Description = description;
		Game = game;
		DetectorModel = detectorModel;
	}
	
	
	private Profile(Guid id, string name, string description)
	{
		Id = id;
		Name = name;
		Description = description;
		Game = null!;
		DetectorModel = null!;
	}
}

internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
	public void Configure(EntityTypeBuilder<Profile> builder) => builder.HasKey(profile => profile.Id);
}