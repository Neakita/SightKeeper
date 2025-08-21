namespace SightKeeper.Application.Windows;

internal static class CondaLocator
{
	public static string CondaActivationBatchFilePath { get; } = Path.Combine(LocateCondaDirectory(), "Scripts", "activate.bat"); 

	private static string UserProfileDirectoryPath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

	private static string[] PossiblePaths =>
	[
		Path.Combine(UserProfileDirectoryPath, "Miniconda3")
	];

	private static string LocateCondaDirectory()
	{
		return PossiblePaths.Single(Directory.Exists);
	}
}