using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Abstract.Interfaces;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Screenshot : Entity, IScreenshot
{
	private const string DirectoryPath = "Data/Images/";
	private const string Extension = ".png";
	
	
	public string FilePath => DirectoryPath + id + Extension;
	
	public DateTime CreationDate { get; private set; }
	
	public Resolution Resolution { get; private set; }

	public Screenshot(Resolution resolution)
	{
		Resolution = resolution;
		CreationDate = DateTime.UtcNow;
	}

	
	protected Screenshot(Guid id) : base(id) =>
		Resolution = new Resolution();
}


internal sealed class ScreenshotConfiguration : IEntityTypeConfiguration<Screenshot>
{
	public void Configure(EntityTypeBuilder<Screenshot> builder)
	{
		builder.OwnsOne(screenshot => screenshot.Resolution);
	}
}