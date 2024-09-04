using System;
using System.Collections;
using System.ComponentModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal partial class TagViewModel : ViewModel, TagData, INotifyDataErrorInfo
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => Validator.ErrorsChanged += value;
		remove => Validator.ErrorsChanged -= value;
	}

	public ViewModelValidator<TagData> Validator { get; }
	public bool HasErrors => Validator.HasErrors;

	public TagViewModel(string name, IValidator<TagData> validator)
	{
		_name = name;
		Validator = new ViewModelValidator<TagData>(validator, this, this);
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		return Validator.GetErrors(propertyName);
	}

	[ObservableProperty] private string _name;
	[ObservableProperty] private Color _color;

	uint TagData.Color => Color.ToUInt32();
}