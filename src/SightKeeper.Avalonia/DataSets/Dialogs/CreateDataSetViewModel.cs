using System;
using System.Collections.Generic;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;
using PlainTagsEditorViewModel = SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain.PlainTagsEditorViewModel;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class CreateDataSetViewModel : DataSetDialogViewModel, NewDataSetData, IDisposable
{
	public override string Header => "Create dataset";

	public override TagsEditorDataContext TagsEditor => _tagsEditor;

	public override DataSetTypePickerViewModel TypePicker { get; } = new();

	public CreateDataSetViewModel()
	{
		_typePickerSubscription = TypePicker.TypeChanged.Subscribe(OnDataSetTypeChanged);
	}

	public void Dispose()
	{
		TypePicker.Dispose();
		_typePickerSubscription.Dispose();
	}

	private readonly IDisposable _typePickerSubscription;
	private TagsEditorDataContext _tagsEditor = new PlainTagsEditorViewModel();

	private void OnDataSetTypeChanged(DataSetType value)
	{
		OnPropertyChanging(nameof(TagsEditor));
		_tagsEditor = value switch
		{
			DataSetType.Classifier or DataSetType.Detector => new PlainTagsEditorViewModel(),
			DataSetType.Poser => new PoserTagsEditorViewModel(),
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		OnPropertyChanged(nameof(TagsEditor));
	}

	public string Name => DataSetEditor.Name;

	public string Description => DataSetEditor.Description;

	public DataSetType Type => TypePicker.SelectedType;
	public IEnumerable<NewTagData> NewTags => ((TagsChanges)_tagsEditor).NewTags;
}