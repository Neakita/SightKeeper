namespace SightKeeper.Application.Training.Data;

public static class DarknetPaths
{
    public const string DarknetDirectory = "Darknet/";
    public const string DarknetExecutablePath = DarknetDirectory + "darknet.exe";
    public const string DataDirectoryPath = DarknetDirectory + "Data/";
    public const string DataFilePath = DataDirectoryPath + "Data.txt";
    public const string ConfigFilePath = DataDirectoryPath + "Config.txt";
    public const string ImagesListFilePath = DataDirectoryPath + "Images.txt";
    public const string ImagesDirectoryPath = DataDirectoryPath + "Images/";
    public const string ClassesListFilePath = DataDirectoryPath + "Classes.txt";
    public const string OutputDirectoryPath = DataDirectoryPath + "Output/";
    public const string BaseWeightsPath = DataDirectoryPath + "base.weights";
}