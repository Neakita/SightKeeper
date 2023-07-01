namespace SightKeeper.Domain.Model.Abstract;

public abstract class Asset
{
    public Screenshot Screenshot { get; private set; }
    
    internal abstract Model Model { get; }
    
    protected Asset(Screenshot screenshot)
    {
        Screenshot = screenshot;
    }
    
    protected Asset()
    {
        Screenshot = null!;
    }
}