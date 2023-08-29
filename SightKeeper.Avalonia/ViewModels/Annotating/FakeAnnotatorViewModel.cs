using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NSubstitute;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

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

    public DataSetViewModel? SelectedDataSet { get; set; }

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

    public DrawerViewModel DrawerViewModel
    {
        get
        {
            var dataSet = DataSetViewModels.First().DataSet;
            var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
            var asset = dataSet.MakeAsset(screenshot);
            var itemClass = dataSet.CreateItemClass("Class 1");
            var item = asset.CreateItem(itemClass, new Bounding(0.1f, 0.1f, 0.5f, 0.3f));
            var substitute = Substitute.For<DrawerViewModel>();
            substitute.Items.Returns(new ObservableCollection<DetectorItemViewModel>()
            {
                new(item, Substitute.For<DetectorItemResizer>(), substitute)
            });
            return substitute;
        }
    }
}

