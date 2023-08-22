using CommunityToolkit.Diagnostics;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public interface AnnotatorEnvironment
{
    AnnotatorTools Tools { get; }
    AnnotatorWorkSpace WorkSpace { get; }
    DataSetViewModel? DataSetViewModel { get; set; }
}

public sealed class AnnotatorEnvironment<TAsset> : AnnotatorEnvironment where TAsset : Asset
{
    public AnnotatorTools Tools => _tools;
    public AnnotatorWorkSpace WorkSpace => _workSpace;

    public DataSetViewModel? DataSetViewModel
    {
        get => _dataSetViewModel;
        set
        {
            if (value != null)
                Guard.IsOfType<DataSetViewModel<TAsset>>(value);
            var castedValue = (DataSetViewModel<TAsset>?)value;
            _dataSetViewModel = castedValue;
            _tools.DataSetViewModel = castedValue;
            _workSpace.DataSetViewModel = castedValue;
        }
    }

    public AnnotatorEnvironment(AnnotatorTools<TAsset> tools, AnnotatorWorkSpace<TAsset> workSpace)
    {
        _tools = tools;
        _workSpace = workSpace;
    }

    private readonly AnnotatorTools<TAsset> _tools;
    private readonly AnnotatorWorkSpace<TAsset> _workSpace;
    private DataSetViewModel<TAsset>? _dataSetViewModel;
}