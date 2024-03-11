using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.Prediction;

public interface Detector
{
	Weights? Weights { get; set; }
	float ProbabilityThreshold { get; set; }
	float IoU { get; set; }
	ImmutableList<DetectionItem> Detect(byte[] image);
	Task<ImmutableList<DetectionItem>> DetectAsync(byte[] image, CancellationToken cancellationToken = default);
}