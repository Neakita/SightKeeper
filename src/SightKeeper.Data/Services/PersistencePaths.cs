namespace SightKeeper.Data.Services;

internal static class PersistencePaths
{
	public static string FilePath => Path.GetFullPath("App.data");
	public static string BackupFilePath => Path.GetFullPath("AppBackup.data");
}