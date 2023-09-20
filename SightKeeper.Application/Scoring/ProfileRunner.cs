using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Scoring;

public interface ProfileRunner
{
    void Run(Profile profile);
    void Stop();
}