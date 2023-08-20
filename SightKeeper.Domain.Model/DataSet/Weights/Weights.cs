namespace SightKeeper.Domain.Model;

public sealed class Weights
{
    public byte[] Data { get; private set; }
    public ModelSize Size { get; private set; }
    public uint Epoch { get; private set; }
    public float Loss { get; private set; }
    
    public Weights(byte[] data, ModelSize size, uint epoch, float loss)
    {
        Data = data;
        Size = size;
        Epoch = epoch;
        Loss = loss;
    }

    private Weights()
    {
        Data = null!;
    }
}