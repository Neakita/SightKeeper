using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using SightKeeper.Application.DataSets;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class ImportDataSetCommand(DataSetImporter setImporter) : AsyncCommand
{
	protected override async Task ExecuteAsync()
	{
		var storageProvider = App.StorageProvider;
		_pickerOptions.Title = "Data set import";
		var files = await storageProvider.OpenFilePickerAsync(_pickerOptions);
		foreach (var file in files)
			await setImporter.Import(file.Path.LocalPath);
	}
	
	private readonly FilePickerOpenOptions _pickerOptions =  new()
	{
		AllowMultiple = true,
		FileTypeFilter =
		[
			new FilePickerFileType("Zip archive")
			{
				Patterns = ["*.zip"]
			}
		]
	};
}