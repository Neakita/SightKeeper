using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class GenericAnnotatorEnvironment<TDataSet> : AnnotatorEnvironment<TDataSet> where TDataSet : DataSet
{
    public AnnotatorTools Tools { get; }
    public AnnotatorWorkSpace WorkSpace { get; }

    public GenericAnnotatorEnvironment(AnnotatorTools<TDataSet> tools, AnnotatorWorkSpace<TDataSet> workSpace)
    {
        Tools = tools;
        WorkSpace = workSpace;
    }
}