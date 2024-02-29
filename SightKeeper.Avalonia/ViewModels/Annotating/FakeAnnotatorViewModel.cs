using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class FakeAnnotatorViewModel : IAnnotatorViewModel
{
    public IReadOnlyCollection<DataSetViewModel> DataSetViewModels
    {
        get
        {
            var dataSet = new DataSet("Some data set");
            dataSet.CreateItemClass("Class 1", 0);
            dataSet.CreateItemClass("Class 2", 0);
            return new DataSetViewModel[]
            {
                new(dataSet, Substitute.For<WeightsDataAccess>()),
                new(new DataSet("Another data set"), Substitute.For<WeightsDataAccess>())
            };
        }
    }

    public SelectedDataSetViewModel? SelectedDataSet { get; set; }

    public bool CanChangeSelectedDataSet => true;
    public AnnotatorScreenshotsViewModel Screenshots => Substitute.For<AnnotatorScreenshotsViewModel>();

    public ScreenshoterViewModel Screenshoter => Substitute.For<ScreenshoterViewModel>();

    public AnnotatorToolsViewModel ToolsViewModel
    {
        get
        {
            var substitute = Substitute.For<AnnotatorToolsViewModel>();
            substitute.ItemClasses.Returns(DataSetViewModels.First().ItemClasses);
            return substitute;
        }
    }

    public DrawerViewModel DrawerViewModel => Substitute.For<DrawerViewModel>();
    public AutoAnnotationViewModel AutoAnnotationViewModel { get; }
    public ViewSettingsViewModel ViewSettingsViewModel { get; }
}

