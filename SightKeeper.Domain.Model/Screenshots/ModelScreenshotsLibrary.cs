namespace SightKeeper.Domain.Model;

public sealed class ModelScreenshotsLibrary : ScreenshotsLibrary
{
    public DataSet DataSet { get; private set; }
    
    internal ModelScreenshotsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
    }

    private ModelScreenshotsLibrary()
    {
        DataSet = null!;
    }
}