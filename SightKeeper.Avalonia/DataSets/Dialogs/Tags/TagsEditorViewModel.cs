using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal partial class TagsEditorViewModel : ViewModel, IDisposable
{
	public BehaviorObservable<bool> IsValid => _isValid;
	public IReadOnlyCollection<TagDataViewModel> Tags => _tags;
	public IReadOnlyCollection<RemovedTag> RemovedTags => _removedTags;

	public TagsEditorViewModel()
	{
		Validator = new NewTagDataValidator(Tags);
	}

	public TagsEditorViewModel(IEnumerable<Tag> existingTags)
	{
		Validator = new NewTagDataValidator(Tags);
		AddTags(existingTags.Select(existingTag => new EditedTagViewModel(Validator, existingTag)));
	}

	public void Dispose()
	{
		foreach (var tag in Tags)
		{
			tag.PropertyChanged -= OnTagPropertyChanged;
			tag.ErrorsChanged -= OnTagErrorsChanged;
		}
	}

	protected readonly NewTagDataValidator Validator;

	protected virtual TagDataViewModel CreateTagViewModel(string name, NewTagDataValidator validator)
	{
		return new TagDataViewModel(name, validator);
	}

	protected void UpdateIsValid()
	{
		bool isValid = Tags.All(IsTagValid);
		if (IsValid != isValid)
			_isValid.OnNext(isValid);
	}

	protected virtual bool IsTagValid(TagDataViewModel newTag)
	{
		return !newTag.HasErrors;
	}

	protected void AddTags(IEnumerable<TagDataViewModel> tags)
	{
		_tags.AddRange(tags);
	}

	private readonly BehaviorSubject<bool> _isValid = new(true);
	private readonly AvaloniaList<TagDataViewModel> _tags = new();
	private readonly List<RemovedTag> _removedTags = new();

	[RelayCommand(CanExecute = nameof(CanAddTag))]
	private void AddTag(string name)
	{
		TagDataViewModel newTag = CreateTagViewModel(name, Validator);
		newTag.PropertyChanged += OnTagPropertyChanged;
		newTag.ErrorsChanged += OnTagErrorsChanged;
		_tags.Add(newTag);
	}

	private bool CanAddTag(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && Tags.All(tag => tag.Name != name);
	}

	private void OnTagErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		UpdateIsValid();
	}

	private void OnTagPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(TagDataViewModel.Name))
			return;
		Guard.IsNotNull(sender);
		var changedTag = (TagDataViewModel)sender;
		foreach (var tag in Tags.Except(changedTag))
			tag.Validator.ValidateProperty(nameof(TagDataViewModel.Name));
	}
}