using CommunityToolkit.Diagnostics;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Application.Training;

public sealed class MutableCompositeTrainingArtifactsProvider : TrainingArtifactsProvider, IDisposable
{
    public ReadOnlyObservableList<TrainingArtifact> Artifacts { get; }

    public MutableCompositeTrainingArtifactsProvider()
    {
        _providers = new ObservableList<TrainingArtifactsProvider>();
        Artifacts = _providers.TransformMany(provider => provider.Artifacts).ToObservableList();
    }

    public void Add(TrainingArtifactsProvider provider)
    {
        Guard.IsFalse(_providers.Contains(provider));
        _providers.Add(provider);
    }

    public void Remove(TrainingArtifactsProvider provider)
    {
        bool isRemoved = _providers.Remove(provider);
        Guard.IsTrue(isRemoved);
    }

    public void Dispose()
    {
        _providers.Dispose();
        Artifacts.Dispose();
    }

    private readonly ObservableList<TrainingArtifactsProvider> _providers;
}