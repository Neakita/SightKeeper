using System.Collections.Generic;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal sealed class DetectorDataSetEditorViewModel : SpecificDataSetEditorViewModel
{
	public override string Header => "Detector";
	public override DataSetType DataSetType => DataSetType.Detector;
	public override IReadOnlyCollection<TagData> Tags { get; }

	public DetectorDataSetEditorViewModel() : base(true)
	{
	}
}