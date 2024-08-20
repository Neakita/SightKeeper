using System.Collections.Immutable;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.DataSets.Dialogs.Specific;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class CreateDataSetViewModel : DialogViewModel<bool>
{
	public override string Header => "Create dataset";
	protected override bool DefaultResult => false;

	public GeneralDataSetEditorViewModel GeneralDataSetEditor { get; }
	public ImmutableArray<SpecificDataSetEditorViewModel> SpecificDataSetEditors { get; } =
	[
		new ClassifierDataSetEditorViewModel(),
		new DetectorDataSetEditorViewModel(),
		new Poser2DDataSetEditorViewModel(),
		new Poser3DDataSetEditorViewModel()
	];

	public CreateDataSetViewModel(GeneralDataSetEditorViewModel generalDataSetEditor)
	{
		GeneralDataSetEditor = generalDataSetEditor;
		_specificDataSetEditor = SpecificDataSetEditors.First();
	}

	[ObservableProperty]
	private SpecificDataSetEditorViewModel _specificDataSetEditor;
}