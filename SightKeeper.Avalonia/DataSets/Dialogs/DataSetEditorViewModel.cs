using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetEditorViewModel : ViewModel, DataSetData, INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => _validator.ErrorsChanged += value;
		remove => _validator.ErrorsChanged -= value;
	}

	public IReadOnlyCollection<Game> Games { get; }

	public bool HasErrors => _validator.HasErrors;

	public DataSetEditorViewModel(ReadDataAccess<Game> gamesDataAccess, IValidator<DataSetData> validator)
	{
		Games = gamesDataAccess.Items;
		_validator = new ViewModelValidator<DataSetData>(validator, this, this);
	}

	public DataSetEditorViewModel(ReadDataAccess<Game> gamesDataAccess, IValidator<DataSetData> validator, DataSet dataSet)
	{
		Games = gamesDataAccess.Items;
		_name = dataSet.Name;
		_description = dataSet.Description;
		_game = dataSet.Game;
		_validator = new ViewModelValidator<DataSetData>(validator, this, this);
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

	[ObservableProperty] private string _name = string.Empty;
	[ObservableProperty] private string _description = string.Empty;
	[ObservableProperty] private Game? _game;
}