namespace SightKeeper.Application.DataSets;

public interface DataSetImporter
{
	Task Import(string filePath);
}