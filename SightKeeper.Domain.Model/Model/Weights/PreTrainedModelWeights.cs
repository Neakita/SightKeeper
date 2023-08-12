namespace SightKeeper.Domain.Model;

public sealed class PreTrainedModelWeights : ModelWeights
{
    public DateTime AddedDate { get; private set; }
    
    public PreTrainedModelWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        ModelWeightsLibrary library,
        DateTime addedDate)
        : base(data, trainedDate, config, library)
    {
        AddedDate = addedDate;
    }

    private PreTrainedModelWeights()
    {
    }
}