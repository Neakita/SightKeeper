using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ModelWeights
{
    public int Batch { get; private set; }
    public DateTime Date { get; private set; }
    public byte[] Data { get; private set; }
    public ICollection<DetectorAsset> Assets { get; private set; }
    public ModelConfig? Config { get; private set; }

    public ModelWeights(int batch, byte[] data, IEnumerable<DetectorAsset> assets, ModelConfig? config = null)
        : this(batch, DateTime.Now, data, assets.ToList(), config) { }
    
    public ModelWeights(int batch, DateTime date, byte[] data, ICollection<DetectorAsset> assets, ModelConfig? config)
    {
        Batch = batch;
        Date = date;
        Data = data;
        Assets = assets;
        Config = config;
    }

    private ModelWeights()
    {
        Data = null!;
        Assets = null!;
    }
}