using System.Diagnostics.CodeAnalysis;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ModelScreenshotsLibrary : ScreenshotsLibrary
{
    internal Abstract.Model Model { get; private set; }

    public ModelScreenshotsLibrary(Abstract.Model model)
    {
        Model = model;
    }

    public override bool CanAddScreenshot(Screenshot screenshot, [NotNullWhen(false)] out string? message)
    {
        if (!base.CanAddScreenshot(screenshot, out message)) return false;
        if (Model is DetectorModel detector && detector.Assets.Any(asset => asset.Screenshot == screenshot))
            message = "Screenshot already added via asset";
        return message == null;
    }

    private ModelScreenshotsLibrary()
    {
        Model = null!;
    }
}