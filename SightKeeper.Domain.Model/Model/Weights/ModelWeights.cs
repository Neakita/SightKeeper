using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public sealed class ModelWeights
{
    public int Batch { get; private set; }
    public DateTime Date { get; private set; }
    public byte[] Data { get; private set; }
    public ICollection<Asset> Assets { get; private set; }
    public ModelConfig? Config { get; private set; }
    internal ModelWeightsLibrary? Library { get; set; }

    public ModelWeights(Model model, int batch, byte[] data, IEnumerable<Asset> assets, ModelConfig? config = null)
        : this(model, batch, DateTime.Now, data, assets.ToList(), config) { }

    private ModelWeights(Model model, int batch, DateTime date, byte[] data, ICollection<Asset> assets, ModelConfig? config)
    {
        if (model is DetectorModel detectorModel)
        {
            if (assets.Any(asset => asset is not DetectorAsset detectorAsset || detectorAsset.Model != detectorModel))
                ThrowHelper.ThrowArgumentException(nameof(assets), $"All assets must belong to the model {model}");
        }
        else
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(model), $"Unknown model type {model.GetType()}");

        Batch = batch;
        Date = date;
        Data = data;
        Assets = assets;
        Config = config;
        Library = model.WeightsLibrary;
    }

    private ModelWeights()
    {
        Data = null!;
        Assets = null!;
    }
}