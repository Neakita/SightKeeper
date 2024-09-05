using System;
using System.Collections;
using System.ComponentModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal partial class TagDataViewModel : ViewModel, NewTagData, INotifyDataErrorInfo
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => Validator.ErrorsChanged += value;
		remove => Validator.ErrorsChanged -= value;
	}

	public ViewModelValidator<NewTagData> Validator { get; }
	public virtual bool HasErrors => Validator.HasErrors;

	public TagDataViewModel(string name, IValidator<NewTagData> validator)
	{
		_name = name;
		Validator = new ViewModelValidator<NewTagData>(validator, this, this);
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		return Validator.GetErrors(propertyName);
	}

	[ObservableProperty] private string _name;
	[ObservableProperty] private Color _color;

	uint NewTagData.Color => Color.ToUInt32();
}