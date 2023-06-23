using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public class ModelWeights : Entity
{
    public int Batch { get; private set; }
    public DateTime Date { get; private set; }
    public byte[] Data { get; private set; }
    public ICollection<Screenshot> Screenshots { get; private set; }

    public ModelWeights(int batch, byte[] data, IEnumerable<Screenshot> screenshots)
        : this(batch, DateTime.Now, data, screenshots.ToList()) { }
    
    public ModelWeights(int batch, DateTime date, byte[] data, ICollection<Screenshot> screenshots)
    {
        Batch = batch;
        Date = date;
        Data = data;
        Screenshots = screenshots;
    }
}