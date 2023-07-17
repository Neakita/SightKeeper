using SightKeeper.Application.Model;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Services;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeModelEditorViewModel : Dialogs.ModelEditorViewModel
{
    public FakeModelEditorViewModel() : base(new ModelDataValidator(), new RegisteredGamesService(new DbGamesDataAccess(new AppDbContext()), new ProcessesAvailableGamesProvider(new DbGamesDataAccess(new AppDbContext()))))
    {
        Name = "Some model";
    }
}