using CommunityToolkit.Diagnostics;
using MemoryPack;

namespace SightKeeper.Data.Services;

public sealed class AppDataAccess : DataSaver
{
	public string FilePath { get; set; } = Path.GetFullPath("App.data");
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
		File.WriteAllBytes(FilePath, serializedData);
	}
}