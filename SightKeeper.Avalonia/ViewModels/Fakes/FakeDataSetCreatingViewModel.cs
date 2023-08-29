using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetCreatingViewModel : Dialogs.DataSetCreatingViewModel
{
    public FakeDataSetCreatingViewModel() : base(Substitute.For<IValidator<NewDataSetInfo>>(), Substitute.For<RegisteredGamesService>())
    {
        Name = "Some data set";
    }
}