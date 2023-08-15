namespace SightKeeper.Application.Scoring;

public interface Detector
{
    IReadOnlyCollection<DetectionItem> Detect(byte[] image);
}