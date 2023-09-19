using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Scoring;

public interface Detector
{
    Weights? Weights { get; set; }
    float ProbabilityThreshold { get; set; }
    float IoU { get; set; }
    Task<IReadOnlyCollection<DetectionItem>> Detect(byte[] image, CancellationToken cancellationToken);
}