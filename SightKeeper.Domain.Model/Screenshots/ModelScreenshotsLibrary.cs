namespace SightKeeper.Domain.Model;

public sealed class ModelScreenshotsLibrary : ScreenshotsLibrary
{
    public Model Model { get; private set; }
    
    internal ModelScreenshotsLibrary(Model model)
    {
        Model = model;
    }

    private ModelScreenshotsLibrary()
    {
        Model = null!;
    }
}