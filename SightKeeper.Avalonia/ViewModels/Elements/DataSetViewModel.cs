using System;
using System.Collections.Generic;
using DynamicData;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class DataSetViewModel : ViewModel, IDisposable
{
    private static readonly string[] Properties =
    {
        nameof(Name),
        nameof(Description),
        nameof(Game),
        nameof(Resolution)
    };
    
    public DataSet DataSet { get; }
    public string Name => DataSet.Name;
    public string Description => DataSet.Description;
    public Game? Game => DataSet.Game;
    public ushort Resolution => DataSet.Resolution;
    public IReadOnlyCollection<ItemClass> ItemClasses { get; }

    public DataSetViewModel(DataSet dataSet)
    {
        DataSet = dataSet;
        _itemClasses.Connect().Bind(out var itemClasses).Subscribe();
        _itemClasses.AddOrUpdate(dataSet.ItemClasses);
        ItemClasses = itemClasses;
    }

    public void NotifyChanges()
    {
        OnPropertiesChanged(Properties);
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