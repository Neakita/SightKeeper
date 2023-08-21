using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Extensions;

public static class ScreenshotsLibraryExtensions
{
    public static DataSet<TAsset> GetDataSet<TAsset>(this ScreenshotsLibrary library) where TAsset : Asset
    {
        Guard.IsNotNull(library.DataSet);
        Guard.IsOfType<DataSet<TAsset>>(library.DataSet);
        return (DataSet<TAsset>)library.DataSet;
    }
}