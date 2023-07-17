namespace SightKeeper.Application.Model.Creating;

public interface ModelCreator
{
    Task<Domain.Model.Model> CreateModel(NewModelDataDTO data, CancellationToken cancellationToken = default);
}