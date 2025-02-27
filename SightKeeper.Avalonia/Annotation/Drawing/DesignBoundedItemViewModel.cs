using Avalonia.Media;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DesignBoundedItemViewModel : BoundedItemDataContext
{
	public Tag Tag
	{
		get
		{
			DetectorDataSet dataSet = new();
			var tag = dataSet.TagsLibrary.CreateTag("Cop");
			tag.Color = Color.FromRgb(0xF0, 0x22, 0x22).ToUInt32();
			return tag;
		}
	}

	public Bounding Bounding { get; set; } = new(0.1, 0.1, 0.9, 0.9);
	public Vector2<double> Position => Bounding.Position;
}