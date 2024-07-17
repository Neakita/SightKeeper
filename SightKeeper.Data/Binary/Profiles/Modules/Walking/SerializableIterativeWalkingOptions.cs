using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
internal sealed partial class SerializableIterativeWalkingOptions : SerializablePassiveWalkingOptions
{
	public Vector2<float> OffsetStep { get; }
	public Vector2<byte> MaximumSteps { get; }

	[MemoryPackConstructor]
	public SerializableIterativeWalkingOptions(Vector2<float> offsetStep, Vector2<byte> maximumSteps)
	{
		OffsetStep = offsetStep;
		MaximumSteps = maximumSteps;
	}

	public SerializableIterativeWalkingOptions(IterativeWalkingOptions options)
	{
		OffsetStep = options.OffsetStep;
		MaximumSteps = options.MaximumSteps;
	}
}