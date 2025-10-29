namespace SightKeeper.Application.Linux;

internal static class CondaLocator
{
	public static string CondaActivationBatchFilePath { get; } = Path.Combine(LocateCondaDirectory(), "bin", "activate"); 

	private static string UserProfileDirectoryPath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

	private static string[] PossiblePaths =>
	[
		Path.Combine(UserProfileDirectoryPath, "miniconda3"),
		Path.Combine(UserProfileDirectoryPath, "Programs", "miniconda3")
	];

	private static string LocateCondaDirectory()
	{
		return PossiblePaths.Single(Directory.Exists);
	}
}