using SightKeeper.Domain;

namespace SightKeeper.Application.Training.Data.Transforming;

internal sealed class RandomItemsCropSettings
{
	public Vector2<ushort> TargetSize { get; set; } = new(320, 320);
	public double IoUThreshold { get; set; } = .2;
	public byte MaximumSamplesPerItem { get; set; } = 10;
	public double MaximumInItemOffset { get; set; } = 1;
	public double MaximumOutItemOffset { get; set; } = .75;
	public Random Random { get; set; } = new(0);
}