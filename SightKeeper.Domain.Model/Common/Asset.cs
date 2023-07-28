namespace SightKeeper.Domain.Model.Common;

public abstract class Asset
{
    public Screenshot Screenshot { get; private set; }
    
    protected Asset(Screenshot screenshot)
    {
        Screenshot = screenshot;
    }
    
    protected Asset()
    {
        Screenshot = null!;
    }
}