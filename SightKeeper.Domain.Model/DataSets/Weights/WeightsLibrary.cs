using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class WeightsLibrary : ObservableObject
{
    public Id Id { get; private set; }
    public DataSet DataSet { get; private set; }
    public IReadOnlyCollection<Weights> Weights => _weights;

    internal WeightsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
        _weights = new ObservableCollection<Weights>();
    }

    public Weights CreateWeights(
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        WeightsMetrics metrics,
        IEnumerable<ItemClass> itemClasses)
    {
        Weights weights = new(this, onnxData, ptData, size, metrics, itemClasses);
        _weights.Add(weights);
        return weights;
    }

    public void RemoveWeights(Weights weights) => Guard.IsTrue(_weights.Remove(weights));

    private readonly ObservableCollection<Weights> _weights;

    private WeightsLibrary()
    {
        DataSet = null!;
        _weights = null!;
    }
}