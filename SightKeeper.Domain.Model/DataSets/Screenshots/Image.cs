using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public sealed class Image : ObservableObject
{
    public Id Id { get; private set; }
    public byte[] Content { get; private set; }

    internal Image(byte[] content)
    {
        Content = content;
    }
}