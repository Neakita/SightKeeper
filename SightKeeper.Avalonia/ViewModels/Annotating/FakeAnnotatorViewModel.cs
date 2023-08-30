using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class FakeAnnotatorViewModel : IAnnotatorViewModel
{
    public IReadOnlyCollection<DataSetViewModel> DataSetViewModels
    {
        get
        {
            var dataSet = new DataSet("Some data set");
            dataSet.CreateItemClass("Class 1");
            dataSet.CreateItemClass("Class 2");
            return new DataSetViewModel[]
            {
                new(dataSet),
                new(new DataSet("Another data set"))
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
}

