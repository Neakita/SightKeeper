namespace SightKeeper.Domain.Model;

public sealed class PreTrainedWeights : Weights
{
    public DateTime AddedDate { get; private set; }
    
    public PreTrainedWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig model,
        WeightsLibrary library,
        DateTime addedDate)
        : base(data, trainedDate, model, library)
    {
        AddedDate = addedDate;
    }

    private PreTrainedWeights()
    {
    }
}