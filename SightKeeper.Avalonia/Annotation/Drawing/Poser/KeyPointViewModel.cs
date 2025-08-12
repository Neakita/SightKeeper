using Avalonia.Media;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public abstract class KeyPointViewModel : ViewModel, KeyPointDataContext
{
	public abstract PoserItemViewModel Item { get; }
	public abstract KeyPoint Value { get; }
	public abstract Tag Tag { get; }

	public Vector2<double> Position
	{
		get => Value.Position;
		set => SetProperty(Value.Position, value, Value, SetKeyPointPosition);
	}

	private void SetKeyPointPosition(KeyPoint keyPoint, Vector2<double> position)
	{
		keyPoint.Position = position;
	}

	public string Name => Tag.Name;
	public Color Color => Color.FromUInt32(Tag.Color);
}