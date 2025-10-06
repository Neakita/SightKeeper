using System;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class DataSetTypeViewModel(
	string name,
	DataSetFactory<Tag, Asset> factory,
	Func<TagsEditorDataContext> tagsEditorFactory) : DataSetTypeDataContext
{
	public string Name => name;
	public DataSetFactory<Tag, Asset> Factory => factory;
	public TagsEditorDataContext TagsEditorDataContext => tagsEditorFactory();
}