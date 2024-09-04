using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using Avalonia.Collections;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Avalonia.Extensions;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed partial class TagsEditorViewModel : ViewModel, IDisposable
{
	public BehaviorObservable<bool> IsValid => _isValid;
	public IReadOnlyCollection<TagViewModel> Tags => _tags;

	public void Dispose()
	{
		foreach (var tag in Tags)
		{
			tag.PropertyChanged -= OnTagPropertyChanged;
			tag.ErrorsChanged -= OnTagErrorsChanged;
		}
	}

	private readonly BehaviorSubject<bool> _isValid = new(true);
	private readonly AvaloniaList<TagViewModel> _tags = new();

	[RelayCommand(CanExecute = nameof(CanAddTag))]
	private void AddTag(string name)
	{
		TagDataValidator validator = new(Tags);
		TagViewModel tag = new(name, validator);
		tag.PropertyChanged += OnTagPropertyChanged;
		tag.ErrorsChanged += OnTagErrorsChanged;
		_tags.Add(tag);
	}

	private void OnTagErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		bool isValid = Tags.All(tag => !tag.HasErrors);
		if (IsValid != isValid)
			_isValid.OnNext(isValid);
	}

	private void OnTagPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(TagViewModel.Name))
			return;
		Guard.IsNotNull(sender);
		var changedTag = (TagViewModel)sender;
		foreach (var tag in Tags.Except(changedTag))
			tag.Validator.ValidateProperty(nameof(TagViewModel.Name));
	}

	private bool CanAddTag(string name)
	{
		return !string.IsNullOrWhiteSpace(name) && Tags.All(tag => tag.Name != name);
	}
}