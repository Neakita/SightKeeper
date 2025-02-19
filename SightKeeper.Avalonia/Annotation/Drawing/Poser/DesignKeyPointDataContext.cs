using Avalonia.Media;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class DesignKeyPointDataContext : KeyPointDataContext
{
	public Tag Tag
	{
		get
		{
			Poser2DDataSet dataSet = new();
			var poserTag = dataSet.TagsLibrary.CreateTag("");
			var keyPointTag = poserTag.CreateKeyPointTag("Head");
			keyPointTag.Color = Color.FromRgb(0xF0, 0x22, 0x22).ToUInt32();
			return keyPointTag;
		}
	}

	public Vector2<double> Position { get; set; }
}