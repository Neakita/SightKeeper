using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Abstract.Interfaces;

namespace SightKeeper.DAL.Members.Common;

public sealed class Resolution : IResolution
{
	public ushort Width { get; }
	public ushort Height { get; }
	
	public Resolution(ushort width = 320, ushort height = 320)
	{
		if (!IsValid(width)) throw new ArgumentException("The width must be a multiple of 32", nameof(width));
		if (!IsValid(height)) throw new ArgumentException("The height must be a multiple of 32", nameof(height));
		
		Width = width;
		Height = height;
	}

	private static bool IsValid(ushort value) => value % 32 == 0;
}


internal sealed class ResolutionConfiguration : IEntityTypeConfiguration<Resolution>
{
	public void Configure(EntityTypeBuilder<Resolution> builder)
	{
		builder.Property(resolution => resolution.Width);
		builder.Property(resolution => resolution.Height);
	}
}