using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;

internal sealed partial class PoserTagsEditorViewModel : ViewModel, PoserTagsEditorDataContext, TagsChanges
{
	public IReadOnlyCollection<EditablePoserTagDataContext> Tags => _tags;
	ICommand TagsEditorDataContext.CreateTagCommand => CreateTagCommand;

	public PoserTagsEditorViewModel(IEnumerable<DomainPoserTag> existingTags) : this()
	{
		foreach (var tag in existingTags)
		{
			ExistingPoserTagViewModel tagViewModel = new(tag);
			_tags.Add(tagViewModel);
		}
	}

	public PoserTagsEditorViewModel()
	{
	}

	private readonly AvaloniaList<EditablePoserTagDataContext> _tags = new();

	[RelayCommand(CanExecute = nameof(CanCreateTag))]
	private void CreateTag(string name)
	{
		NewPoserTagViewModel tag = new()
		{
			Name = name
		};
		_tags.Add(tag);
	}

	private bool CanCreateTag(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && Tags.All(tag => tag.Name != name);
	}

	public IEnumerable<DomainTag> RemovedTags => Enumerable.Empty<DomainTag>();

	public IEnumerable<EditedTagData> EditedTags =>
		_tags.OfType<ExistingPoserTagViewModel>().Where(tag => tag.IsEffectivelyEdited);

	public IEnumerable<NewTagData> NewTags => _tags.OfType<NewPoserTagViewModel>();
}