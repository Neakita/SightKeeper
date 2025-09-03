using Avalonia.Media;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed class KeyPointViewModel : ViewModel, KeyPointDataContext
{
	public PoserItemViewModel Item { get; }
	public KeyPoint Value { get; }
	public Tag Tag => Value.Tag;

	public Vector2<double> Position
	{
		get => Value.Position;
		set => SetProperty(Value.Position, value, Value, SetKeyPointPosition);
	}

	public string Name => Tag.Name;
	public Color Color => Color.FromUInt32(Tag.Color);

	public KeyPointViewModel(PoserItemViewModel item, KeyPoint value)
	{
		Item = item;
		Value = value;
	}

	private void SetKeyPointPosition(KeyPoint keyPoint, Vector2<double> position)
	{
		keyPoint.Position = position;
	}
}