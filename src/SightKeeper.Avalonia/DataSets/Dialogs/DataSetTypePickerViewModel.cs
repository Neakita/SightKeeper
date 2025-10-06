using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetTypePickerViewModel(IReadOnlyCollection<DataSetTypeViewModel> types)
	: ViewModel, DataSetTypePickerDataContext, IDisposable
{
	public IReadOnlyCollection<DataSetTypeViewModel> Types => types;
	[ObservableProperty] public partial DataSetTypeViewModel SelectedType { get; set; } = types.First();
	public IObservable<DataSetTypeViewModel> TypeChanged => _typeChanged.AsObservable();

	IReadOnlyCollection<DataSetTypeDataContext> DataSetTypePickerDataContext.Types => Types;

	DataSetTypeDataContext DataSetTypePickerDataContext.SelectedType
	{
		get => SelectedType;
		set => SelectedType = (DataSetTypeViewModel)value;
	}

	public void Dispose()
	{
		_typeChanged.Dispose();
	}

	private readonly Subject<DataSetTypeViewModel> _typeChanged = new();

	partial void OnSelectedTypeChanged(DataSetTypeViewModel value)
	{
		_typeChanged.OnNext(value);
	}
}