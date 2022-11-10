using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Abstract.Interfaces;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Screenshot : IScreenshot
{
	private const string DirectoryPath = "Data/Images/";
	private const string Extension = ".png";
	
	public Guid Id { get; }

	public DateTime CreationDate { get; }
	
	public Resolution Resolution { get; private set; }
	
	public string FilePath => DirectoryPath + Id + Extension;
	
	
	public Screenshot() : this(new Resolution()) { }

	public Screenshot(Resolution resolution)
	{
		Resolution = resolution;
		CreationDate = DateTime.UtcNow;
	}


	protected Screenshot(Guid id, DateTime creationDate)
	{
		Id = id;
		CreationDate = creationDate;
		Resolution = new Resolution();
	}
}


internal sealed class ScreenshotConfiguration : IEntityTypeConfiguration<Screenshot>
{
	public void Configure(EntityTypeBuilder<Screenshot> builder)
	{
		builder.HasKey(screenshot => screenshot.Id);
		builder.OwnsOne(screenshot => screenshot.Resolution);
		builder.Property(screenshot => screenshot.CreationDate);
	}
}