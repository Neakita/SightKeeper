using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.Common;

public sealed class Image : ObservableObject
{
    public Id Id { get; private set; }
    public byte[] Content { get; private set; }

    public Image(byte[] content)
    {
        Content = content;
    }
}