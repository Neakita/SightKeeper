using SightKeeper.Application.DataSet;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Data.Services.DataSet;
using SightKeeper.Services;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetCreatingViewModel : Dialogs.DataSetCreatingViewModel
{
    public FakeDataSetCreatingViewModel() : base(new DataSetInfoValidator(new DbDataSetsDataAccess(new AppDbContext())), new RegisteredGamesService(new DbGamesDataAccess(new AppDbContext()), new ProcessesAvailableGamesProvider(new DbGamesDataAccess(new AppDbContext()))))
    {
        Name = "Some model";
    }
}