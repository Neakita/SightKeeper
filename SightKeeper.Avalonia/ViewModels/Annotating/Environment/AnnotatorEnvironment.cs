using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorEnvironment
{
    AnnotatorTools Tools { get; }
    AnnotatorWorkSpace WorkSpace { get; }
}

public interface AnnotatorEnvironment<TDataSet> : AnnotatorEnvironment where TDataSet : DataSet
{
}