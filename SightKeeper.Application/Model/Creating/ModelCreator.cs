namespace SightKeeper.Application.Model.Creating;

public interface ModelCreator
{
    Domain.Model.Model CreateModel(NewModelDataDTO data);
}