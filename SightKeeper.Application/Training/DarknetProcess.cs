﻿using SightKeeper.Application.Training.Data;

namespace SightKeeper.Application.Training;

public interface DarknetProcess
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
    
    IObservable<string> OutputReceived { get; }
    Task RunAsync(DarknetArguments arguments, CancellationToken cancellationToken = default);
}