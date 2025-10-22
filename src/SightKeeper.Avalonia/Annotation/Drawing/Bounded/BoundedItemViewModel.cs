using Avalonia.Media;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

public abstract class BoundedItemViewModel : ViewModel, BoundedItemDataContext
{
	public abstract Tag Tag { get; }
	public abstract DetectorItem Value { get; }

	public Bounding Bounding
	{
		get => Value.Bounding;
		set
		{
			if (SetProperty(Bounding, value, Value, SetItemBounding))
				OnPropertyChanged(nameof(Position));
		}
	}

	public Vector2<double> Position => Bounding.Position;
	public string Name => Tag.Name;
	public Color Color => Color.FromUInt32(Tag.Color);

	private static void SetItemBounding(DetectorItem item, Bounding bounding)
	{
		item.Bounding = bounding;
	}
}