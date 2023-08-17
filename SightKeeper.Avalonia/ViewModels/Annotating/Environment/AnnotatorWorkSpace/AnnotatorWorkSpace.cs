using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorWorkSpace
{
}

public interface AnnotatorWorkSpace<TDataSet> : AnnotatorWorkSpace where TDataSet : DataSet
{
}