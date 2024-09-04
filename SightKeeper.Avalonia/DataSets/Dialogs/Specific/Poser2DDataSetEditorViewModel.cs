using System.Collections.Generic;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Specific;

internal sealed class Poser2DDataSetEditorViewModel : SpecificDataSetEditorViewModel
{
	public override string Header => "Poser 2D";
	public override DataSetType DataSetType => DataSetType.Poser2D;
	public override IReadOnlyCollection<TagData> Tags { get; }

	public Poser2DDataSetEditorViewModel() : base(true)
	{
	}
}