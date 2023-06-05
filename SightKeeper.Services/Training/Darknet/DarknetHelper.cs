﻿using SightKeeper.Application.Training;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services.Training.Darknet;

public class DarknetHelper
{
	public const string DarknetDirectory = "Darknet/";
	private const string DarknetExecutablePath = DarknetDirectory + "darknet.exe";
	private const string DataDirectoryPath = DarknetDirectory + "Data/";
	private const string DataFilePath = DataDirectoryPath + "Data.txt";
	private const string ConfigFilePath = DataDirectoryPath + "Config.txt";
	private const string ImagesListFilePath = DataDirectoryPath + "Images.txt";
	private const string ImagesDirectoryPath = DataDirectoryPath + "Images/";
	private const string ClassesListFilePath = DataDirectoryPath + "Classes.txt";
	private const string OutputDirectoryPath = DataDirectoryPath + "Output/";

	public static void ClearDataDirectory()
	{
		foreach (string file in Directory.EnumerateFiles(DataFilePath, "*", SearchOption.AllDirectories))
			File.Delete(file);
	}

	public DarknetHelper(ImagesExporter<DetectorModel> imagesExporter)
	{
		_imagesExporter = imagesExporter;
	}

	public DarknetProcess StartNewTrainer(DetectorModel model)
	{
		IReadOnlyCollection<string> images = _imagesExporter.Export(ImagesDirectoryPath, model);
		PrepareImagesListFile(images);
		PrepareDirectories();
		PrepareClassesList(model.ItemClasses);
		PrepareDataFile((byte) model.ItemClasses.Count);
		PrepareConfig(model);
		DarknetProcess result = new(DarknetExecutablePath);
		DarknetArguments arguments = new()
		{
			ModelType = ModelType.Detector,
			DataPath = DataFilePath.Replace(DarknetDirectory, string.Empty),
			ConfigPath = ConfigFilePath.Replace(DarknetDirectory, string.Empty),
			DoNotShow = false
		};
		result.Run(arguments);
		return result;
	}
	
	private readonly ImagesExporter<DetectorModel> _imagesExporter;

	private static void PrepareDirectories()
	{
		Directory.CreateDirectory(ImagesDirectoryPath);	
		Directory.CreateDirectory(OutputDirectoryPath);	
	}
	
	private static void PrepareClassesList(IReadOnlyCollection<ItemClass> itemClasses)
	{
		string fileContent = string.Join('\n', itemClasses.Select(itemClass => itemClass.Name));
		File.WriteAllText(ClassesListFilePath, fileContent);
	}

	private static void PrepareDataFile(byte classesCount)
	{
		DarknetData data = new()
		{
			ClassesCount = classesCount,
			ImagesListPath = ImagesListFilePath.Replace(DarknetDirectory, string.Empty),
			ClassesListPath = ClassesListFilePath.Replace(DarknetDirectory, string.Empty),
			OutputPath = OutputDirectoryPath.Replace(DarknetDirectory, string.Empty)
		};
		File.WriteAllText(DataFilePath, data.ToString());
	}

	private static void PrepareConfig(DetectorModel model)
	{
		if (model.Config == null) throw new NullReferenceException("Config is null");
		DetectorConfigParameters parameters = new(64, 16, model.Resolution.Width, model.Resolution.Width, (ushort) model.ItemClasses.Count);
		string fileContent = parameters.Deploy(model.Config);
		File.WriteAllText(ConfigFilePath, fileContent);
	}

	private static void PrepareImagesListFile(IReadOnlyCollection<string> paths)
	{
		string content = string.Join('\n', paths);
		File.WriteAllText(ImagesListFilePath, content.Replace(DarknetDirectory, string.Empty));
	}
}