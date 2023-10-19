using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Prediction;

public interface ProfileRunner
{
    void Run(Profile profile);
    void Stop();
}