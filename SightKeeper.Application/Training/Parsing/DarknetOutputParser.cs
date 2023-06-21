namespace SightKeeper.Application.Training.Parsing;

public interface DarknetOutputParser<TModel>
{
    bool TryParse(string output, out TrainingProgress? progress);
}