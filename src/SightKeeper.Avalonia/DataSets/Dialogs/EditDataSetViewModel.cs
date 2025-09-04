using System;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class EditDataSetViewModel : DataSetDialogViewModel, ExistingDataSetData
{
	public override string Header => $"Edit {DataSet.Name}";
	public override TagsEditorDataContext TagsEditor { get; }
	public DataSet DataSet { get; }

	public EditDataSetViewModel(DataSet dataSet) : base(dataSet)
	{
		DataSet = dataSet;
		TagsEditor = dataSet switch
		{
			DataSet<ClassifierAsset> or DataSet<ItemsAsset<DetectorItem>> => new PlainTagsEditorViewModel(dataSet.TagsLibrary.Tags),
			PoserDataSet poserDataSet => new PoserTagsEditorViewModel(poserDataSet.TagsLibrary.Tags),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	public string Name => DataSetEditor.Name;

	public string Description => DataSetEditor.Description;

	public TagsChanges TagsChanges => (TagsChanges)TagsEditor;
}