using System;
using System.Collections;
using System.ComponentModel;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels;

internal sealed class ValidatableGameViewModel : GameViewModel, INotifyDataErrorInfo
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => _validator.ErrorsChanged += value;
		remove => _validator.ErrorsChanged -= value;
	}

	public bool HasErrors => _validator.HasErrors;

	public ValidatableGameViewModel(
		Game game,
		GameIconProvider iconProvider,
		GameExecutableDisplayer executableDisplayer,
		IValidator<Game> validator)
		: base(game, iconProvider, executableDisplayer)
	{
		_validator = new ViewModelValidator<Game>(validator, this, game);
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		return _validator.GetErrors(propertyName);
	}

	private readonly ViewModelValidator<Game> _validator;
}