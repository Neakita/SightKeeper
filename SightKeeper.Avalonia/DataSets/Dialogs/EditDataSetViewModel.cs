using System;
using System.Linq;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;
using PlainTagsEditorViewModel = SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain.PlainTagsEditorViewModel;

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
			ClassifierDataSet or DetectorDataSet => new PlainTagsEditorViewModel(dataSet.TagsLibrary.Tags),
			Poser2DDataSet or Poser3DDataSet => new PoserTagsEditorViewModel(dataSet.TagsLibrary.Tags.Cast<PoserTag>()),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
	}

	public string Name => DataSetEditor.Name;

	public string Description => DataSetEditor.Description;

	public TagsChanges TagsChanges => (TagsChanges)TagsEditor;
}