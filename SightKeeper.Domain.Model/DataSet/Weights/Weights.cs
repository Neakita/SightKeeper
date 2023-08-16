namespace SightKeeper.Domain.Model;

public abstract class Weights
{
    public byte[] Data { get; private set; }
    public DateTime TrainedDate { get; private set; }
    public ModelConfig Model { get; private set; }
    public WeightsLibrary? Library { get; internal set; }

    protected Weights(byte[] data, DateTime trainedDate, ModelConfig model, WeightsLibrary library)
    {
        Data = data;
        TrainedDate = trainedDate;
        Model = model;
        Library = library;
    }

    protected Weights()
    {
        Data = null!;
        Model = null!;
        Library = null!;
    }
}