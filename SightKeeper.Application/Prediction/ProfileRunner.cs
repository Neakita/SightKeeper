using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application.Prediction;

public interface ProfileRunner
{
    void Run(Profile profile);
    void Stop();
}