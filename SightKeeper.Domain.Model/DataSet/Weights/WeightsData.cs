using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.DataSet.Weights;

public abstract class WeightsData : ObservableObject
{
    public Id Id { get; private set; }
    public byte[] Content { get; private set; }

    protected WeightsData(byte[] content)
    {
        Content = content;
    }
}