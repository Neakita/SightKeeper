using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class SerializablePoser3DKeyPoint
{
	public static SerializablePoser3DKeyPoint Create(KeyPoint3D keyPoint)
	{
		return new SerializablePoser3DKeyPoint(keyPoint.Position, keyPoint.IsVisible);
	}

	public Vector2<double> Position { get; }
	public bool IsVisible { get; }

	public SerializablePoser3DKeyPoint(Vector2<double> position, bool isVisible)
	{
		Position = position;
		IsVisible = isVisible;
	}
}