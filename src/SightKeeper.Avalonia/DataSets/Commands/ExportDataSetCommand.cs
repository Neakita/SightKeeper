using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using SightKeeper.Application.DataSets;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class ExportDataSetCommand(DataSetExporter<DataSet<Asset>> exporter) : CancellableAsyncCommand<DataSet<Asset>>
{
	protected override async Task ExecuteAsync(DataSet<Asset> set, CancellationToken cancellationToken)
	{
		var storageProvider = App.StorageProvider;
		_pickerOptions.Title = $"{set.Name} export";
		_pickerOptions.SuggestedFileName = $"{set.Name}.zip";
		var storageFile = await storageProvider.SaveFilePickerAsync(_pickerOptions);
		if (storageFile == null)
			return;
		var path = storageFile.Path.LocalPath;
		await exporter.ExportAsync(path, set, cancellationToken);
	}

	private readonly FilePickerSaveOptions _pickerOptions = new()
	{
		DefaultExtension = ".zip",
		FileTypeChoices =
		[
			new FilePickerFileType(".zip")
			{
				Patterns = ["*.zip"]
			}
		],
		ShowOverwritePrompt = true
	};
}