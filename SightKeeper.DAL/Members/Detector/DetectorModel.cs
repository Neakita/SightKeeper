using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Abstract;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Detector;

public sealed class DetectorModel : Model
{
	public override IEnumerable<Screenshot> Screenshots => DetectorScreenshots.Cast<Screenshot>().ToList();
	
	public List<DetectorScreenshot> DetectorScreenshots { get; private set; } = new();

	
	public DetectorModel(string name) : this(name, new Resolution()) { }
	public DetectorModel(string name, Resolution resolution) : base(name, resolution) { }


	private DetectorModel(Guid id, string name) : base(id, name) { }
}


internal sealed class DetectorModelConfiguration : IEntityTypeConfiguration<DetectorModel>
{
	public void Configure(EntityTypeBuilder<DetectorModel> builder) => builder.ToTable("DetectorModels");
}