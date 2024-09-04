using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.DataSets.Dialogs.Specific;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class CreateDataSetViewModel : DialogViewModel<bool>, IDisposable
{
	public override string Header => "Create dataset";
	protected override bool DefaultResult => false;

	public DataSetEditorViewModel DataSetEditor { get; }

	public ImmutableArray<SpecificDataSetEditorViewModel> SpecificDataSetEditors { get; } =
	[
		new ClassifierDataSetEditorViewModel(),
		new DetectorDataSetEditorViewModel(),
		new Poser2DDataSetEditorViewModel(),
		new Poser3DDataSetEditorViewModel()
	];

	public CreateDataSetViewModel(DataSetEditorViewModel dataSetDataSetEditor)
	{
		DataSetEditor = dataSetDataSetEditor;
		DataSetEditor.Resolution = DataSet.DefaultResolution;
		_specificDataSetEditor = SpecificDataSetEditors.First();
		DataSetEditor.ErrorsChanged += OnDataSetEditorErrorsChanged;
		_constructorDisposable = SpecificDataSetEditors.Select(editor => editor.IsValid).Cast<IObservable<bool>>()
			.Aggregate((x, y) => x.Merge(y)).Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}

	public void Dispose()
	{
		DataSetEditor.Dispose();
		DataSetEditor.ErrorsChanged -= OnDataSetEditorErrorsChanged;
		foreach (var editor in SpecificDataSetEditors)
			editor.Dispose();
		_constructorDisposable.Dispose();
	}

	[ObservableProperty] private SpecificDataSetEditorViewModel _specificDataSetEditor;
	private IDisposable _constructorDisposable;

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
		return !DataSetEditor.HasErrors && SpecificDataSetEditor.IsValid;
	}
}