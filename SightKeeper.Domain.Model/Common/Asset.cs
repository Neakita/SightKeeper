using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public abstract class Asset
{
    internal static TAsset Create<TAsset>(Screenshot screenshot) where TAsset : Asset
    {
        if (typeof(TAsset) == typeof(DetectorAsset))
            return (new DetectorAsset(screenshot) as TAsset)!;
        return ThrowHelper.ThrowInvalidOperationException<TAsset>();
    }
    
    public Screenshot Screenshot { get; private set; }

    internal abstract bool IsUsesItemClass(ItemClass itemClass);
    
    protected Asset(Screenshot screenshot)
    {
        Screenshot = screenshot;
    }
    
    protected Asset()
    {
        Screenshot = null!;
    }
}