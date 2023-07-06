namespace SightKeeper.Domain.Model.Common;

public sealed class ModelScreenshotsLibrary : ScreenshotsLibrary
{
    internal Abstract.Model Model { get; private set; }
    
    internal ModelScreenshotsLibrary(Abstract.Model model)
    {
        Model = model;
    }

    private ModelScreenshotsLibrary()
    {
        Model = null!;
    }
}