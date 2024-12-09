using System;
using System.Linq;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class EditDataSetViewModel : DataSetDialogViewModel
{
	public override string Header => $"Edit {_dataSet.Name}";
	public override TagsEditorViewModel TagsEditor { get; }

	public EditDataSetViewModel(DataSet dataSet, DataSetEditorViewModel editor) : base(editor)
	{
		_dataSet = dataSet;
		TagsEditor = dataSet switch
		{
			ClassifierDataSet or DetectorDataSet => new TagsEditorViewModel(dataSet.TagsLibrary.Tags),
			Poser2DDataSet or Poser3DDataSet => new PoserTagsEditorViewModel(dataSet.TagsLibrary.Tags.Cast<PoserTag>()),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};
		_tagsEditorSubscription = TagsEditor.IsValid.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
	}

	public override void Dispose()
	{
		TagsEditor.Dispose();
		_tagsEditorSubscription.Dispose();
	}

	private readonly IDisposable _tagsEditorSubscription;
	private readonly DataSet _dataSet;
}