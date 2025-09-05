using System;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class EditDataSetViewModel : DataSetDialogViewModel, ExistingDataSetData
{
	public override string Header => $"Edit {DataSet.Name}";
	public override TagsEditorDataContext TagsEditor { get; }
	public DataSet<Tag, Asset> DataSet { get; }

	public EditDataSetViewModel(DataSet<Tag, Asset> dataSet) : base(dataSet)
	{
		DataSet = dataSet;
		TagsEditor = dataSet switch
		{
			DataSet<PoserTag, Asset> poserDataSet => new PoserTagsEditorViewModel(poserDataSet.TagsLibrary.Tags),
			not null => new PlainTagsEditorViewModel(dataSet.TagsLibrary.Tags),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet), dataSet, null)
		};
	}

	public string Name => DataSetEditor.Name;

	public string Description => DataSetEditor.Description;

	public TagsChanges TagsChanges => (TagsChanges)TagsEditor;
}