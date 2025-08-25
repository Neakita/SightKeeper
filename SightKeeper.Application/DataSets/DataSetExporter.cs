namespace SightKeeper.Application.DataSets;

public interface DataSetExporter<in TDataSet>
{
	Task ExportAsync(string path, TDataSet set, CancellationToken cancellationToken);
}