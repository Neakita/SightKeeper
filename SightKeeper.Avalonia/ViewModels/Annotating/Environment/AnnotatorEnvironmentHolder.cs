using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorEnvironmentHolder : ViewModel
{
    public AnnotatorEnvironment? CurrentEnvironment
    {
        get => _currentEnvironment;
        private set => SetProperty(ref _currentEnvironment, value);
    }
    
    public AnnotatorEnvironmentHolder(IComponentContext context)
    {
        _context = context;
    }

    private readonly IComponentContext _context;
    [ObservableProperty] private ModelType? _dataSetModelType;
    private AnnotatorEnvironment? _currentEnvironment;
    
    partial void OnDataSetModelTypeChanged(ModelType? value) =>
        CurrentEnvironment = ResolveAnnotatorEnvironment(value);

    private AnnotatorEnvironment? ResolveAnnotatorEnvironment(ModelType? modelType) => modelType switch
    {
        null => null,
        ModelType.Detector => _context.Resolve<AnnotatorEnvironment<DetectorAsset>>(),
        _ => ThrowHelper.ThrowArgumentOutOfRangeException<AnnotatorEnvironment?>(nameof(modelType), modelType, "Unexpected data set model type")
    };
}