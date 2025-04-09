using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

public sealed partial class DataSetTypePickerViewModel : ViewModel, IDisposable
{
	public static ImmutableArray<DataSetType> Types { get; } =
	[
		DataSetType.Classifier,
		DataSetType.Detector,
		DataSetType.Poser2D,
		DataSetType.Poser3D
	];

	[ObservableProperty] public partial DataSetType SelectedType { get; set; }
	public IObservable<DataSetType> TypeChanged => _typeChanged.AsObservable();

	public void Dispose()
	{
		_typeChanged.Dispose();
	}

	private readonly Subject<DataSetType> _typeChanged = new();

	partial void OnSelectedTypeChanged(DataSetType value)
	{
		_typeChanged.OnNext(value);
	}
}