using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.DataSet.Weights;

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
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<ItemClass> itemClasses)
    {
        Weights weights = new(this, onnxData, ptData, size, epoch, boundingLoss, classificationLoss, deformationLoss, itemClasses);
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