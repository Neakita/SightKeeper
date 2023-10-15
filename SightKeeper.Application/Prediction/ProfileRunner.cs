using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Prediction;

public interface ProfileRunner
{
    public bool MakeScreenshots { get; set; }
    public float MinimumProbability { get; set; }
    public float MaximumProbability { get; set; }
    public byte MaximumFPS { get; set; }
    void Run(Profile profile);
    void Stop();
}