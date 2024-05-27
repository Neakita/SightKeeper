using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

internal sealed class DataSetViewModel : ViewModel, IDisposable
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
    public IReadOnlyCollection<Weights> Weights { get; }

    public DataSetViewModel(DataSet dataSet, WeightsDataAccess weightsDataAccess)
    {
        DataSet = dataSet;
        _itemClasses.Connect()
            .Bind(out var itemClasses)
            .Subscribe()
            .DisposeWith(_constructorDisposables);
        _itemClasses.AddOrUpdate(dataSet.ItemClasses);
        ItemClasses = itemClasses;
        _weights.AddRange(DataSet.Weights);
        _weights.Connect()
            .Bind(out var weights)
            .Subscribe()
            .DisposeWith(_constructorDisposables);
        Weights = weights;
        weightsDataAccess.WeightsCreated
            .Where(w => w.Library.DataSet == DataSet)
            .Subscribe(w => _weights.Add(w))
            .DisposeWith(_constructorDisposables);
        weightsDataAccess.WeightsRemoved
            .Where(w => w.Library.DataSet == DataSet)
            .Subscribe(w => _weights.Remove(w))
            .DisposeWith(_constructorDisposables);
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

    private readonly CompositeDisposable _constructorDisposables = new();
    private readonly SourceCache<ItemClass, string> _itemClasses = new(itemClass => itemClass.Name);
    private readonly SourceList<Weights> _weights = new();

    private void UpdateItemClasses() =>
        _itemClasses.Edit(items =>
        {
            items.Clear();
            items.AddOrUpdate(DataSet.ItemClasses);
        });
}