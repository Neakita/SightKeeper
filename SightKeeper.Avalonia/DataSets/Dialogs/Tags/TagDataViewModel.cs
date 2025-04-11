using System;
using System.Collections;
using System.ComponentModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal partial class TagDataViewModel : ViewModel, EditableTagDataContext, NewTagData, INotifyDataErrorInfo
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => Validator.ErrorsChanged += value;
		remove => Validator.ErrorsChanged -= value;
	}

	[ObservableProperty] public partial string Name { get; set; }
	[ObservableProperty] public partial Color Color { get; set; }

	public ViewModelValidator<NewTagData> Validator { get; }
	public virtual bool HasErrors => Validator.HasErrors;

	public TagDataViewModel(string name, IValidator<NewTagData> validator)
	{
		Name = name;
		Validator = new ViewModelValidator<NewTagData>(validator, this, this);
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		return Validator.GetErrors(propertyName);
	}

	uint NewTagData.Color => Color.ToUInt32();
}