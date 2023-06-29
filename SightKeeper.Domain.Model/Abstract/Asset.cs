namespace SightKeeper.Domain.Model.Abstract;

public abstract class Asset
{
    public Screenshot Screenshot { get; private set; }
    
    public Asset(Screenshot screenshot)
    {
        Screenshot = screenshot;
    }
    
    protected Asset()
    {
        Screenshot = null!;
    }
}