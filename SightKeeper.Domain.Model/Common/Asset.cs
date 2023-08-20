using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public abstract class Asset
{
    internal static TAsset Create<TAsset>(DataSet<TAsset> dataSet, Screenshot screenshot) where TAsset : Asset
    {
        if (typeof(TAsset) == typeof(DetectorAsset))
            return (new DetectorAsset(dataSet, screenshot) as TAsset)!;
        return ThrowHelper.ThrowInvalidOperationException<TAsset>();
    }
    
    public Screenshot Screenshot { get; private set; }
    public DataSet DataSet { get; private set; }
    public AssetUsage Usage { get; set; }

    internal abstract bool IsUsesItemClass(ItemClass itemClass);
    
    protected Asset(DataSet dataSet, Screenshot screenshot)
    {
        DataSet = dataSet;
        Screenshot = screenshot;
    }
    
    protected Asset()
    {
        DataSet = null!;
        Screenshot = null!;
    }
}