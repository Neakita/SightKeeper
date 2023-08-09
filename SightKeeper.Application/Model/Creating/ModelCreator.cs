namespace SightKeeper.Application.Model.Creating;

public interface ModelCreator
{
    IObservable<SightKeeper.Domain.Model.Model> ModelCreated { get; }
    Task<Domain.Model.Model> CreateModel(NewModelDataDTO data, CancellationToken cancellationToken = default);
}