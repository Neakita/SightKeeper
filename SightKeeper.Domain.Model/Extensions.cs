using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public static class Extensions
{
    public static ModelType GetDomainType(this DataSet dataSet) => dataSet switch
    {
        DetectorDataSet => ModelType.Detector,
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<ModelType>(nameof(dataSet), $"Unexpected model type: {dataSet.GetType()}")
    };

    public static DataSet GetDataSet(this ScreenshotsLibrary library)
    {
        Guard.IsOfType<ModelScreenshotsLibrary>(library);
        var modelLibrary = (ModelScreenshotsLibrary)library;
        return modelLibrary.DataSet;
    }

    public static TModel GetDataSet<TModel>(this ScreenshotsLibrary library) where TModel : DataSet
    {
        var model = library.GetDataSet();
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