using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using SightKeeper.Application.DataSets;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class ExportDataSetCommand(DataSetExporter exporter) : CancellableAsyncCommand<DataSet>
{
	protected override async Task ExecuteAsync(DataSet set, CancellationToken cancellationToken)
	{
		var storageProvider = ((IClassicDesktopStyleApplicationLifetime)global::Avalonia.Application.Current.ApplicationLifetime).MainWindow.StorageProvider;
		var filePickerOptions = new FilePickerSaveOptions
		{
			Title = $"{set.Name} export",
			SuggestedFileName = $"{set.Name}.zip",
			DefaultExtension = ".zip",
			FileTypeChoices = [new FilePickerFileType(".zip")],
			ShowOverwritePrompt = true
		};
		var storageFile = await storageProvider.SaveFilePickerAsync(filePickerOptions);
		if (storageFile == null)
			return;
		var stream = await storageFile.OpenWriteAsync();
		await exporter.ExportAsync(stream, set, cancellationToken);
	}
}