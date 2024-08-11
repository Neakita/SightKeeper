using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DataSetViewModel : ViewModel, IDisposable
{
    private static readonly string[] Properties =
    [
	    nameof(Name),
        nameof(Description),
        nameof(Game),
        nameof(Resolution)
    ];
    
    public DataSet DataSet { get; }
    public string Name => DataSet.Name;
    public string Description => DataSet.Description;
    public Game? Game => DataSet.Game;
    public ushort Resolution => DataSet.Resolution;
    public IReadOnlyCollection<Tag> Tags { get; }
    public IReadOnlyCollection<Weights> Weights { get; }

    public DataSetViewModel(DataSet dataSet, WeightsDataAccess weightsDataAccess)
    {
	    throw new NotImplementedException();
	    // DataSet = dataSet;
	    // _tags.Connect()
	    //     .Bind(out var tags)
	    //     .Subscribe()
	    //     .DisposeWith(_constructorDisposables);
	    // _tags.AddOrUpdate(dataSet.Tags);
	    // Tags = tags;
	    // _weights.AddRange(DataSet.Weights);
	    // _weights.Connect()
	    //     .Bind(out var weights)
	    //     .Subscribe()
	    //     .DisposeWith(_constructorDisposables);
	    // Weights = weights;
	    // weightsDataAccess.WeightsCreated
	    //     .Where(data => objectsLookupper.GetDataSet(data.library) == DataSet)
	    //     .Subscribe(data => _weights.Add(data.weights))
	    //     .DisposeWith(_constructorDisposables);
	    // weightsDataAccess.WeightsRemoved
	    //     .Where(w => objectsLookupper.GetDataSet(objectsLookupper.GetLibrary(w)) == DataSet)
	    //     .Subscribe(w => _weights.Remove(w))
	    //     .DisposeWith(_constructorDisposables);
    }

    public void NotifyChanges()
    {
        OnPropertiesChanged(Properties);
        UpdateTags();
    }

    public void Dispose()
    {
        _tags.Dispose();
    }

    public override string ToString() => Name;

    private readonly CompositeDisposable _constructorDisposables = new();
    private readonly SourceCache<Tag, string> _tags = new(tag => tag.Name);
    private readonly SourceList<Weights> _weights = new();

    private void UpdateTags() =>
        _tags.Edit(items =>
        {
            items.Clear();
            items.AddOrUpdate(DataSet.Tags);
        });
}