using System;
using SightKeeper.Application;
using SightKeeper.Avalonia.DataSets.Dialogs.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class DataSetTypeViewModel(
	string name,
	Factory<DataSet<Tag, Asset>> dataSetFactory,
	Func<TagsEditorDataContext> tagsEditorFactory) : DataSetTypeDataContext
{
	public string Name => name;
	public Factory<DataSet<Tag, Asset>> DataSetFactory => dataSetFactory;
	public TagsEditorDataContext TagsEditorDataContext => tagsEditorFactory();
}