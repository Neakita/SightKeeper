namespace SightKeeper.Application.Training.Data;

public static class DarknetPaths
{
    public const string DarknetDirectory = "Darknet/";
    public const string DataDirectoryPath = "Data/";
    public const string DarknetExecutablePath = DarknetDirectory + "darknet.exe";
    public const string DarknetDataDirectoryPath = DarknetDirectory + DataDirectoryPath;
    public const string DataFilePath = DarknetDataDirectoryPath + "Data.txt";
    public const string ConfigFilePath = DarknetDataDirectoryPath + "Config.txt";
    public const string ImagesListFilePath = DarknetDataDirectoryPath + "Images.txt";
    public const string ImagesDirectoryPath = DarknetDataDirectoryPath + "Images/";
    public const string ClassesListFilePath = DarknetDataDirectoryPath + "Classes.txt";
    public const string OutputDirectoryPath = DarknetDataDirectoryPath + "Output/";
    public const string DarknetBaseWeightsPath = DarknetDataDirectoryPath + BaseWeightsFileName;
    public const string BaseWeightsPath = DataDirectoryPath + BaseWeightsFileName;
    private const string BaseWeightsFileName = "base.weights";
}