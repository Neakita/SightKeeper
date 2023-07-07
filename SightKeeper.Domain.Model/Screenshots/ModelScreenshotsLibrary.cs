namespace SightKeeper.Domain.Model;

public sealed class ModelScreenshotsLibrary : ScreenshotsLibrary
{
    internal Model Model { get; private set; }
    
    internal ModelScreenshotsLibrary(Model model)
    {
        Model = model;
    }

    private ModelScreenshotsLibrary()
    {
        Model = null!;
    }
}