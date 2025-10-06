using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using PlainTagsEditorViewModel = SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain.PlainTagsEditorViewModel;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class CreateDataSetViewModel : DataSetDialogViewModel, NewDataSetData, IDisposable
{
	public override string Header => "Create dataset";

	public override TagsEditorDataContext TagsEditor => _tagsEditor;

	public override DataSetTypePickerViewModel TypePicker { get; }

	public CreateDataSetViewModel(DataSetTypePickerViewModel typePicker)
	{
		TypePicker = typePicker;
		TypePicker.TypeChanged
			.Subscribe(OnDataSetTypeChanged)
			.DisposeWith(_disposable);
	}

	public void Dispose()
	{
		TypePicker.Dispose();
		_disposable.Dispose();
	}

	private readonly CompositeDisposable _disposable = new();
	private TagsEditorDataContext _tagsEditor = new PlainTagsEditorViewModel();

	private void OnDataSetTypeChanged(DataSetTypeViewModel value)
	{
		OnPropertyChanging(nameof(TagsEditor));
		_tagsEditor = value.TagsEditorDataContext;
		OnPropertyChanged(nameof(TagsEditor));
	}

	public string Name => DataSetEditor.Name;

	public string Description => DataSetEditor.Description;

	public DataSetFactory<Tag, Asset> Factory => TypePicker.SelectedType.Factory;
	public IEnumerable<NewTagData> NewTags => ((TagsChanges)_tagsEditor).NewTags;
}