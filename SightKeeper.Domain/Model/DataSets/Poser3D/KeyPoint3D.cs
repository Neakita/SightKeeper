using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class KeyPoint3D : KeyPoint
{
	public override Poser3DItem Item { get; }
	public override KeyPointTag3D Tag { get; }
	public bool IsVisible { get; set; }

	internal KeyPoint3D(Vector2<double> position, Poser3DItem item, KeyPointTag3D tag, bool isVisible) : base(position)
	{
		Item = item;
		Tag = tag;
		IsVisible = isVisible;
	}
}