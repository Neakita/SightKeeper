using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public static class Extensions
{
    public static ModelType GetDomainType(this Model model) => model switch
    {
        DetectorModel => ModelType.Detector,
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<ModelType>(nameof(model), $"Unexpected model type: {model.GetType()}")
    };

    public static Model GetModel(this ScreenshotsLibrary library)
    {
        Guard.IsOfType<ModelScreenshotsLibrary>(library);
        var modelLibrary = (ModelScreenshotsLibrary)library;
        return modelLibrary.Model;
    }

    public static TModel GetModel<TModel>(this ScreenshotsLibrary library) where TModel : Model
    {
        var model = library.GetModel();
        Guard.IsOfType<TModel>(model);
        return (TModel)model;
    }

    public static TAsset GetAsset<TAsset>(this Screenshot screenshot) where TAsset : Asset
    {
        Guard.IsNotNull(screenshot.Asset);
        Guard.IsOfType<TAsset>(screenshot.Asset);
        return (TAsset)screenshot.Asset;
    }

    public static TAsset? GetOptionalAsset<TAsset>(this Screenshot screenshot) where TAsset : Asset
    {
        if (screenshot.Asset != null)
            Guard.IsOfType<TAsset>(screenshot.Asset);
        return (TAsset?)screenshot.Asset;
    }
}