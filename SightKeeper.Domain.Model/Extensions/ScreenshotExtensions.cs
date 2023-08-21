using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Extensions;

public static class ScreenshotExtensions
{
    public static TAsset GetAsset<TAsset>(this Screenshot screenshot) where TAsset : Asset
    {
        Guard.IsNotNull(screenshot.Asset);
        Guard.IsOfType<TAsset>(screenshot.Asset);
        return (TAsset)screenshot.Asset;
    }
    
    public static TAsset? GetOptionalAsset<TAsset>(this Screenshot screenshot) where TAsset : Asset => screenshot.Asset switch
    {
        null => null,
        TAsset asset => asset,
        _ => ThrowExpectedTypeMismatchException<TAsset>(screenshot.Asset)
    };

    private static TAsset ThrowExpectedTypeMismatchException<TAsset>(Asset asset,
        [CallerArgumentExpression(nameof(asset))] string name = "") => ThrowHelper.ThrowArgumentException<TAsset>(name,
        $"Asset expected to be of type {typeof(TAsset)}, but was of type {asset.GetType()}");
}