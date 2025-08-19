using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using SightKeeper.Application.DataSets;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class ImportDataSetCommand(DataSetImporter setImporter) : AsyncCommand
{
	protected override async Task ExecuteAsync()
	{
		var storageProvider = ((IClassicDesktopStyleApplicationLifetime)global::Avalonia.Application.Current.ApplicationLifetime).MainWindow.StorageProvider;
		var filePickerOptions = new FilePickerOpenOptions
		{
			Title = "Data set import",
			AllowMultiple = true,
			FileTypeFilter =
			[
				new FilePickerFileType("Zip archive")
				{
					Patterns = ["*.zip"]
				}
			]
		};
		var files = await storageProvider.OpenFilePickerAsync(filePickerOptions);
		foreach (var file in files)
			await setImporter.Import(file.Path.LocalPath);
	}
}