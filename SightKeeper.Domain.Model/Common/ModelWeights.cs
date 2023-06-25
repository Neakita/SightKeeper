using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ModelWeights : Entity
{
    public int Batch { get; private set; }
    public DateTime Date { get; private set; }
    public byte[] Data { get; private set; }
    public ICollection<DetectorAsset> Assets { get; private set; }

    public ModelWeights(int batch, byte[] data, IEnumerable<DetectorAsset> assets)
        : this(batch, DateTime.Now, data, assets.ToList()) { }
    
    public ModelWeights(int batch, DateTime date, byte[] data, ICollection<DetectorAsset> assets)
    {
        Batch = batch;
        Date = date;
        Data = data;
        Assets = assets;
    }
}