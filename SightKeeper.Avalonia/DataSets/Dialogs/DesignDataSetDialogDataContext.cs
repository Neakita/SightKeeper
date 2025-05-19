using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class DesignDataSetDialogDataContext : DataSetDialogDataContext
{
	public static DesignDataSetDialogDataContext DetectorCreation => new()
	{
		DataSetEditor = new DesignDataSetEditorDataContext
		{
			Name = "KF2 Detector",
			Description =
				"""
				Detector dataset for Killing Floor 2 with tags for each individual type of ZEDs and bosses.
				The limbs should not be included in the frame of the object.
				Only the body and head should be included in the frame of the object.
				"""
		},
		TagsEditor = new DesignPlainTagsEditorDataContext(
			"Cyst", "Alpha Clot", "Slasher", "Stalker", "Crawler", "Gorefast", "Bloat", "Siren",
			"Husk", "E.D.A.R.", "Quarter Pound", "Scrake", "Fleshpound", "Rioter", "Elite Crawler", "Gorefiend",
			"Dr. Hans Volter", "Patriarch", "King Fleshpound", "Abomination", "Matriarch"),
		TypePicker = new DataSetTypePickerViewModel
		{
			SelectedType = DataSetType.Detector
		}
	};

	public required DataSetEditorDataContext DataSetEditor { get; init; }

	public required TagsEditorDataContext TagsEditor { get; init; }

	public DataSetTypePickerViewModel? TypePicker { get; init; }

	public ICommand ApplyCommand => new RelayCommand(() => { });
	public ICommand CloseCommand => new RelayCommand(() => { });
}