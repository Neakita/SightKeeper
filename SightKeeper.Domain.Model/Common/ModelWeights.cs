using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class ModelWeights
{
    public int Batch { get; private set; }
    public DateTime Date { get; private set; }
    public byte[] Data { get; private set; }
    public ICollection<Asset> Assets { get; private set; }
    public ModelConfig? Config { get; private set; }

    public ModelWeights(Abstract.Model model, int batch, byte[] data, IEnumerable<Asset> assets, ModelConfig? config = null)
        : this(model, batch, DateTime.Now, data, assets.ToList(), config) { }

    private ModelWeights(Abstract.Model model, int batch, DateTime date, byte[] data, ICollection<Asset> assets, ModelConfig? config)
    {
        if (assets.Any(asset => asset.Model != model))
            ThrowHelper.ThrowArgumentException(nameof(assets), $"All assets must must belong to the model {model}");
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