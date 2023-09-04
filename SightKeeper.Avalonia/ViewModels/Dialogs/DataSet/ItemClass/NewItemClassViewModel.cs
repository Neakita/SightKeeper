using System.Collections.Generic;
using System.Collections.Immutable;
using Avalonia.Controls;
using Avalonia.Media;
using SightKeeper.Application.DataSet;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;

public sealed class NewItemClassViewModel : EditableItemClass
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

    public NewItemClassViewModel(string name, byte index)
    {
        Name = name;
        Color = ColorPalette[index % ColorPalette.Count];
    }
    
    public ItemClassInfo ToItemClassInfo() => new(Name, Color.ToUInt32());
}