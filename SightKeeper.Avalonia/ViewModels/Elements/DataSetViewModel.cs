using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

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
    public IReadOnlyCollection<Tag> ItemClasses { get; }
    public IReadOnlyCollection<Weights> Weights { get; }

    public DataSetViewModel(DataSet dataSet, WeightsDataAccess weightsDataAccess)
    {
        DataSet = dataSet;
        _itemClasses.Connect()
            .Bind(out var itemClasses)
            .Subscribe()
            .DisposeWithEx(_constructorDisposables);
        _itemClasses.AddOrUpdate(dataSet.ItemClasses);
        ItemClasses = itemClasses;
        _weights.AddRange(DataSet.Weights.Weights);
        _weights.Connect()
            .Bind(out var weights)
            .Subscribe()
            .DisposeWithEx(_constructorDisposables);
        Weights = weights;
        weightsDataAccess.WeightsCreated
            .Where(w => w.Library.DataSet == DataSet)
            .Subscribe(w => _weights.Add(w))
            .DisposeWithEx(_constructorDisposables);
        weightsDataAccess.WeightsDeleted
            .Where(w => w.Library.DataSet == DataSet)
            .Subscribe(w => _weights.Remove(w))
            .DisposeWithEx(_constructorDisposables);
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
    private readonly SourceCache<Tag, string> _itemClasses = new(itemClass => itemClass.Name);
    private readonly SourceList<Weights> _weights = new();

    private void UpdateItemClasses() =>
        _itemClasses.Edit(items =>
        {
            items.Clear();
            items.AddOrUpdate(DataSet.ItemClasses);
        });
}