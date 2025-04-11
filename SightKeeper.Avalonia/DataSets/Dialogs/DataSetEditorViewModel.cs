using System;
using System.Collections;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using SightKeeper.Application.DataSets;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetEditorViewModel : ViewModel, DataSetEditorDataContext, DataSetData, INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => _validator.ErrorsChanged += value;
		remove => _validator.ErrorsChanged -= value;
	}


	[ObservableProperty] public partial string Name { get; set; } = string.Empty;
	[ObservableProperty] public partial string Description { get; set; } = string.Empty;

	public bool HasErrors => _validator.HasErrors;

	public DataSetEditorViewModel(IValidator<DataSetData> validator)
	{
		_validator = new ViewModelValidator<DataSetData>(validator, this, this);
	}

	public DataSetEditorViewModel(IValidator<DataSetData> validator, DataSet dataSet)
	{
		Name = dataSet.Name;
		Description = dataSet.Description;
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
}