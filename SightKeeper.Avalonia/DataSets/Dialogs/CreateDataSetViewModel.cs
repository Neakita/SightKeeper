using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class CreateDataSetViewModel : DialogViewModel<bool>, IDisposable
{
	public override string Header => "Create dataset";
	protected override bool DefaultResult => false;

	public DataSetEditorViewModel DataSetEditor { get; }
	public TagsEditorViewModel TagsEditor { get; } = new();

	public ImmutableArray<DataSetType> DataSetTypes { get; } =
	[
		DataSetType.Classifier,
		DataSetType.Detector,
		DataSetType.Poser2D,
		DataSetType.Poser3D
	];

	public CreateDataSetViewModel(DataSetEditorViewModel dataSetDataSetEditor)
	{
		DataSetEditor = dataSetDataSetEditor;
		DataSetEditor.Resolution = DataSet.DefaultResolution;
		_dataSetType = DataSetTypes.First();
		DataSetEditor.ErrorsChanged += OnDataSetEditorErrorsChanged;
		_constructorDisposable = TagsEditor.IsValid.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}

	public void Dispose()
	{
		DataSetEditor.Dispose();
		DataSetEditor.ErrorsChanged -= OnDataSetEditorErrorsChanged;
		TagsEditor.Dispose();
		_constructorDisposable.Dispose();
	}

	[ObservableProperty] private DataSetType _dataSetType;
	private readonly IDisposable _constructorDisposable;

	private void OnDataSetEditorErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		ApplyCommand.NotifyCanExecuteChanged();
	}

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Guard.IsFalse(DataSetEditor.HasErrors);
		Return(true);
	}

	private bool CanApply()
	{
		return !DataSetEditor.HasErrors && TagsEditor.IsValid;
	}
}