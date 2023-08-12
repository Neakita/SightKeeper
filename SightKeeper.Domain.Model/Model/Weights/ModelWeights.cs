namespace SightKeeper.Domain.Model;

public abstract class ModelWeights
{
    public byte[] Data { get; private set; }
    public DateTime TrainedDate { get; private set; }
    public ModelConfig Config { get; private set; }
    public ModelWeightsLibrary? Library { get; internal set; }

    protected ModelWeights(byte[] data, DateTime trainedDate, ModelConfig config, ModelWeightsLibrary library)
    {
        Data = data;
        TrainedDate = trainedDate;
        Config = config;
        Library = library;
    }

    protected ModelWeights()
    {
        Data = null!;
        Config = null!;
        Library = null!;
    }
}