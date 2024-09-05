using System;
using System.Linq;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;

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
			ClassifierDataSet or DetectorDataSet => new TagsEditorViewModel(dataSet.Tags),
			Poser2DDataSet or Poser3DDataSet => new PoserTagsEditorViewModel(dataSet.Tags.Cast<PoserTag>()),
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