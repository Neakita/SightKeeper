using System.Collections.Generic;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal sealed class Poser3DDataSetEditorViewModel : SpecificDataSetEditorViewModel
{
	public override string Header => "Poser 3D";
	public override DataSetType DataSetType => DataSetType.Poser3D;
	public override IReadOnlyCollection<TagData> Tags { get; }

	public Poser3DDataSetEditorViewModel() : base(true)
	{
	}
}