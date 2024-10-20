using System;
using System.Collections.Generic;
using System.Windows.Input;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal class PoserNewTagViewModel : TagDataViewModel, NewPoserTagData, IDisposable
{
	IReadOnlyCollection<NewTagData> NewPoserTagData.KeyPointTags => _tagsEditor.Tags;

	public ICommand AddKeyPointTagCommand => _tagsEditor.AddTagCommand;
	public IReadOnlyCollection<TagDataViewModel> KeyPointTags => _tagsEditor.Tags;
	public BehaviorObservable<bool> IsKeyPointsValid => _tagsEditor.IsValid;

	public PoserNewTagViewModel(string name, IValidator<NewTagData> validator) : base(name, validator)
	{
	}

	public void Dispose()
	{
		_tagsEditor.Dispose();
	}

	private readonly TagsEditorViewModel _tagsEditor = new();
}