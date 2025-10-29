namespace SightKeeper.Application;

public sealed class CondaLocator(IReadOnlyCollection<string> possiblePaths)
{
	public string CondaActivationBatchFilePath => Path.Combine(LocateCondaDirectory(), "bin", "activate"); 

	private string LocateCondaDirectory()
	{
		return possiblePaths.Single(Directory.Exists);
	}
}