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

	public TagsEditorViewModel TagsEditor
	{
		get => _tagsEditor;
		private set => SetProperty(ref _tagsEditor, value);
	}

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
		_tagsEditorSubscription = TagsEditor.IsValid.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}

	public void Dispose()
	{
		DataSetEditor.Dispose();
		DataSetEditor.ErrorsChanged -= OnDataSetEditorErrorsChanged;
		TagsEditor.Dispose();
		_tagsEditorSubscription.Dispose();
	}

	[ObservableProperty] private DataSetType _dataSetType;
	private IDisposable _tagsEditorSubscription;
	private TagsEditorViewModel _tagsEditor = new();

	partial void OnDataSetTypeChanged(DataSetType value)
	{
		_tagsEditorSubscription.Dispose();
		TagsEditor = value switch
		{
			DataSetType.Classifier or DataSetType.Detector => new TagsEditorViewModel(),
			DataSetType.Poser2D or DataSetType.Poser3D => new PoserTagsEditorViewModel(),
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		_tagsEditorSubscription = TagsEditor.IsValid.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}

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