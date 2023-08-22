using System;
using System.Collections.Generic;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public interface DataSetViewModel
{
    public static DataSetViewModel Create(DataSet dataSet)
    {
        return dataSet switch
        {
            DataSet<DetectorAsset> detectorDataSet => new DataSetViewModel<DetectorAsset>(detectorDataSet),
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<DataSetViewModel>(nameof(dataSet), dataSet.GetType(), "Unknown data set type")
        };
    }
    
    protected static readonly string[] Properties =
    {
        nameof(Name),
        nameof(Description),
        nameof(Game),
        nameof(Resolution)
    };
    
    DataSet DataSet { get; }
    string Name { get; }
    string Description { get; }
    Game? Game { get; }
    Resolution Resolution { get; }
    IReadOnlyCollection<ItemClass> ItemClasses { get; }

    void NotifyDataSetEdited();
}

public sealed class DataSetViewModel<TAsset> : ViewModel, DataSetViewModel, IDisposable where TAsset : Asset
{
    public DataSet<TAsset> DataSet { get; }
    DataSet DataSetViewModel.DataSet => DataSet;
    public string Name => DataSet.Name;
    public string Description => DataSet.Description;
    public Game? Game => DataSet.Game;
    public Resolution Resolution => DataSet.Resolution;
    public IReadOnlyCollection<ItemClass> ItemClasses { get; }

    public DataSetViewModel(DataSet<TAsset> dataSet)
    {
        DataSet = dataSet;
        _itemClasses.Connect().Bind(out var itemClasses).Subscribe();
        ItemClasses = itemClasses;
    }

    public void NotifyDataSetEdited()
    {
        OnPropertiesChanged(DataSetViewModel.Properties);
        UpdateItemClasses();
    }

    public void Dispose()
    {
        _itemClasses.Dispose();
    }

    public override string ToString() => Name;

    private readonly SourceCache<ItemClass, string> _itemClasses = new(itemClass => itemClass.Name);

    private void UpdateItemClasses()
    {
        _itemClasses.Edit(items =>
        {
            items.Clear();
            items.AddOrUpdate(DataSet.ItemClasses);
        });
    }
}