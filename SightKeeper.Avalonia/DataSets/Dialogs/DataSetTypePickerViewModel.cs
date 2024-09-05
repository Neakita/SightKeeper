using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class DataSetTypePickerViewModel : ViewModel, IDisposable
{
	public static ImmutableArray<DataSetType> Types { get; } =
	[
		DataSetType.Classifier,
		DataSetType.Detector,
		DataSetType.Poser2D,
		DataSetType.Poser3D
	];

	public IObservable<DataSetType> TypeChanged => _typeChanged.AsObservable();

	public void Dispose()
	{
		_typeChanged.Dispose();
	}

	private readonly Subject<DataSetType> _typeChanged = new();
	[ObservableProperty] private DataSetType _type;

	partial void OnTypeChanged(DataSetType value)
	{
		_typeChanged.OnNext(value);
	}
}