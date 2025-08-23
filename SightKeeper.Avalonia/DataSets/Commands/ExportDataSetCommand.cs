using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using SightKeeper.Application.DataSets;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class ExportDataSetCommand(DataSetExporter exporter) : CancellableAsyncCommand<DataSet>
{
	protected override async Task ExecuteAsync(DataSet set, CancellationToken cancellationToken)
	{
		var storageProvider = App.StorageProvider;
		_pickerOptions.Title = $"{set.Name} export";
		_pickerOptions.SuggestedFileName = $"{set.Name}.zip";
		var storageFile = await storageProvider.SaveFilePickerAsync(_pickerOptions);
		if (storageFile == null)
			return;
		var stream = await storageFile.OpenWriteAsync();
		await exporter.ExportAsync(stream, set, cancellationToken);
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