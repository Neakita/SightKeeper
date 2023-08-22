using SightKeeper.Application.DataSet;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Services;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetEditorViewModel : Dialogs.DataSetEditorViewModel
{
    public FakeDataSetEditorViewModel() : base(new DataSetInfoValidator(), new RegisteredGamesService(new DbGamesDataAccess(new AppDbContext()), new ProcessesAvailableGamesProvider(new DbGamesDataAccess(new AppDbContext()))))
    {
        Name = "Some model";
    }
}