using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

internal sealed class NewTagViewModel : EditableTagViewModel, NewTagData
{
	uint NewTagData.Color => Color.ToUInt32();
}