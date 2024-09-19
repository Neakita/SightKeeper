using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.DataSets.Compositions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetEditorViewModel : ViewModel, DataSetData, INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => _validator.ErrorsChanged += value;
		remove => _validator.ErrorsChanged -= value;
	}

	public IReadOnlyCollection<Game> Games { get; }

	public IReadOnlyCollection<CompositionViewModel> Compositions => _compositions;

	public bool HasErrors => _validator.HasErrors;

	public DataSetEditorViewModel(GamesDataAccess gamesDataAccess, IValidator<DataSetData> validator)
	{
		Games = gamesDataAccess.Games;
		_validator = new ViewModelValidator<DataSetData>(validator, this, this);
	}

	public DataSetEditorViewModel(GamesDataAccess gamesDataAccess, IValidator<DataSetData> validator, DataSet dataSet)
	{
		Games = gamesDataAccess.Games;
		_name = dataSet.Name;
		_description = dataSet.Description;
		_game = dataSet.Game;
		_composition = CompositionViewModel.Create(dataSet.Composition);
		_validator = new ViewModelValidator<DataSetData>(validator, this, this);
		if (_composition == null)
			return;
		for (var i = 0; i < _compositions.Count; i++)
		{
			if (_compositions[i].GetType() == _composition.GetType())
				_compositions = _compositions.SetItem(i, _composition);
		}
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		return _validator.GetErrors(propertyName);
	}

	public void Dispose()
	{
		_validator.Dispose();
	}

	private readonly ViewModelValidator<DataSetData> _validator;

	private readonly ImmutableList<CompositionViewModel> _compositions =
	[
		new FixedTransparentCompositionViewModel(),
		new FloatingTransparentCompositionViewModel()
	];
	[ObservableProperty] private string _name = string.Empty;
	[ObservableProperty] private string _description = string.Empty;
	[ObservableProperty] private Game? _game;
	[ObservableProperty] private CompositionViewModel? _composition;

	Composition? DataSetData.Composition => Composition?.ToComposition();
}