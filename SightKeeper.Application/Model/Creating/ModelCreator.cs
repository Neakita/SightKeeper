namespace SightKeeper.Application.Model.Creating;

public interface ModelCreator
{
    IObservable<SightKeeper.Domain.Model.DataSet> ModelCreated { get; }
    Task<Domain.Model.DataSet> CreateModel(NewModelDataDTO data, CancellationToken cancellationToken = default);
}