using System;
using System.Collections.Generic;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.DataSets;

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
                new(dataSet, new FakeWeightsDataAccess()),
                new(new DataSet("Another data set"), new FakeWeightsDataAccess())
            };
        }
    }

    public SelectedDataSetViewModel? SelectedDataSet { get; set; }

    public bool CanChangeSelectedDataSet => true;
    public AnnotatorScreenshotsViewModel Screenshots => throw new NotImplementedException();

    public ScreenshoterViewModel Screenshoter => throw new NotImplementedException();

    public AnnotatorToolsViewModel ToolsViewModel
    {
        get
        {
            /*var substitute = Substitute.For<AnnotatorToolsViewModel>();
            substitute.ItemClasses.Returns(DataSetViewModels.First().ItemClasses);
            return substitute;*/
            throw new NotImplementedException();
        }
    }

    public DrawerViewModel DrawerViewModel => throw new NotImplementedException();
    public AutoAnnotationViewModel AutoAnnotationViewModel { get; }
    public ViewSettingsViewModel ViewSettingsViewModel { get; }
}

