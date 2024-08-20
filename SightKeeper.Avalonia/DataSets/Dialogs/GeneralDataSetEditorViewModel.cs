using System.Collections.Generic;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class GeneralDataSetEditorViewModel : ViewModel, GeneralDataSetInfo
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public Game? Game { get; set; }
	
	public IReadOnlyCollection<Game> Games { get; }

	public GeneralDataSetEditorViewModel(GamesDataAccess gamesDataAccess)
	{
		Games = gamesDataAccess.Games;
	}
}