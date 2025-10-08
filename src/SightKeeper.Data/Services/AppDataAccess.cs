using CommunityToolkit.Diagnostics;
using MemoryPack;

namespace SightKeeper.Data.Services;

internal sealed class AppDataAccess : DataSaver
{
	public string FilePath { get; set; } = Path.GetFullPath("App.data");
	public string BackupFilePath { get; set; } = Path.GetFullPath("AppBackup.data");
	internal AppData Data { get; set; } = new();

	public void Load()
	{
		if (!File.Exists(FilePath))
		{
			Data = new AppData();
			return;
		}
		var serializedData = File.ReadAllBytes(FilePath);
		var data = MemoryPackSerializer.Deserialize<AppData>(serializedData);
		Guard.IsNotNull(data);
		Data = data;
	}

	public void Save()
	{
		var serializedData = MemoryPackSerializer.Serialize(Data);
		if (File.Exists(FilePath))
			File.Copy(FilePath, BackupFilePath, true);
		File.WriteAllBytes(FilePath, serializedData);
	}
}