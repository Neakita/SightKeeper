using System.Collections.Generic;
using System.Collections.Immutable;
using Avalonia.Controls;
using Avalonia.Media;
using SightKeeper.Application.DataSets;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tag;

public sealed class NewTagViewModel : EditableTag
{
    private static readonly ImmutableList<Color> ColorPalette = ExtractPalette(new FluentColorPalette()).ToImmutableList();

    private static IEnumerable<Color> ExtractPalette(IColorPalette palette)
    {
        for (var j = 0; j < palette.ShadeCount; j++)
        for (var i = 0; i < palette.ColorCount; i++)
            yield return palette.GetColor(i, j);
    }

    public string Name { get; set; }
    public Color Color { get; set; }

    public NewTagViewModel(string name, byte index)
    {
        Name = name;
        Color = ColorPalette[index % ColorPalette.Count];
    }
    
    public TagInfo ToTagInfo() => new(Name, Color.ToUInt32());
}