using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPoint3DViewModel : KeyPointViewModel
{
	public override Poser3DItemViewModel Item { get; }
	public override KeyPoint3D Value { get; }
	public override Tag Tag => Value.Tag;

	public bool IsVisible
	{
		get => Value.IsVisible;
		set => SetProperty(Value.IsVisible, value, Value, SetKeyPointVisibility);
	}

	private void SetKeyPointVisibility(KeyPoint3D keyPoint, bool isVisible)
	{
		keyPoint.IsVisible = isVisible;
	}

	public KeyPoint3DViewModel(Poser3DItemViewModel item, KeyPoint3D value)
	{
		Item = item;
		Value = value;
	}
}