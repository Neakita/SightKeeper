using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application;

namespace SightKeeper.Data.Services;

public sealed class AppDataAccess
{
	public string FilePath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "App.data");
	public AppData Data { get; set; } = new();

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