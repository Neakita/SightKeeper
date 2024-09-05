using System;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class CreateDataSetViewModel : DataSetDialogViewModel
{
	public override string Header => "Create dataset";

	public override TagsEditorViewModel TagsEditor => _tagsEditor;

	public override DataSetTypePickerViewModel TypePicker { get; } = new();

	public CreateDataSetViewModel(DataSetEditorViewModel dataSetEditor) : base(dataSetEditor)
	{
		_typePickerSubscription = TypePicker.TypeChanged.Subscribe(OnDataSetTypeChanged);
		_tagsEditorSubscription = TagsEditor.IsValid.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}

	public override void Dispose()
	{
		TypePicker.Dispose();
		_typePickerSubscription.Dispose();
		_tagsEditorSubscription.Dispose();
	}

	private readonly IDisposable _typePickerSubscription;
	private IDisposable _tagsEditorSubscription;
	private TagsEditorViewModel _tagsEditor = new();

	private void OnDataSetTypeChanged(DataSetType value)
	{
		_tagsEditorSubscription.Dispose();
		OnPropertyChanging(nameof(TagsEditor));
		_tagsEditor = value switch
		{
			DataSetType.Classifier or DataSetType.Detector => new TagsEditorViewModel(),
			DataSetType.Poser2D or DataSetType.Poser3D => new PoserTagsEditorViewModel(),
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		OnPropertyChanged(nameof(TagsEditor));
		_tagsEditorSubscription = TagsEditor.IsValid.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}
}