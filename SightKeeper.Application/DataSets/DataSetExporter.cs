using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.DataSets;

public interface DataSetExporter
{
	Task ExportAsync(Stream stream, DataSet set, CancellationToken cancellationToken);
}