using System;
using System.Collections.Generic;
using System.Windows.Input;
using FluentValidation;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class PoserTagViewModel : TagViewModel, PoserTagData, IDisposable
{
	IReadOnlyCollection<TagData> PoserTagData.KeyPointTags => _tagsEditor.Tags;

	public ICommand AddKeyPointTagCommand => _tagsEditor.AddTagCommand;
	public IReadOnlyCollection<TagViewModel> KeyPointTags => _tagsEditor.Tags;
	public BehaviorObservable<bool> IsKeyPointsValid => _tagsEditor.IsValid;

	public PoserTagViewModel(string name, IValidator<TagData> validator) : base(name, validator)
	{
	}

	public void Dispose()
	{
		_tagsEditor.Dispose();
	}

	private readonly TagsEditorViewModel _tagsEditor = new();
}