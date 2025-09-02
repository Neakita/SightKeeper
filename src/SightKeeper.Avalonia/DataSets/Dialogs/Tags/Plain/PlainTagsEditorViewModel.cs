using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Plain;

internal sealed partial class PlainTagsEditorViewModel : ViewModel, PlainTagsEditorDataContext, TagsChanges
{
	public BehaviorObservable<bool> IsValid => _isValid;
	public IReadOnlyCollection<EditableTagDataContext> Tags => _tags;
	ICommand TagsEditorDataContext.CreateTagCommand => CreateTagCommand;

	public PlainTagsEditorViewModel()
	{
	}

	public PlainTagsEditorViewModel(IEnumerable<Tag> existingTags) : this()
	{
		foreach (var tag in existingTags)
		{
			ExistingTagViewModel tagViewModel = new(tag);
			_tags.Add(tagViewModel);
		}
	}

	private readonly BehaviorSubject<bool> _isValid = new(true);
	private readonly AvaloniaList<EditableTagViewModel> _tags = new();

	[RelayCommand(CanExecute = nameof(CanCreateTag))]
	private void CreateTag(string name)
	{
		NewTagViewModel newTag = new()
		{
			Name = name
		};
		_tags.Add(newTag);
	}

	private bool CanCreateTag(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && Tags.All(tag => tag.Name != name);
	}

	public IEnumerable<Tag> RemovedTags => Enumerable.Empty<DomainTag>();

	public IEnumerable<EditedTagData> EditedTags =>
		_tags.OfType<ExistingTagViewModel>().Where(tag => tag.IsEffectivelyEdited);

	public IEnumerable<NewTagData> NewTags => _tags.OfType<NewTagViewModel>();
}