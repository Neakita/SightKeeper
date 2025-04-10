using System;
using System.Collections.Generic;
using System.Windows.Input;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags;

internal class PoserNewTagViewModel : TagDataViewModel, EditablePoserTagDataContext, NewPoserTagData, IDisposable
{
	public ICommand CreateKeyPointTagCommand => _tagsEditor.CreateTagCommand;
	public IReadOnlyCollection<TagDataViewModel> KeyPointTags => _tagsEditor.Tags;
	public BehaviorObservable<bool> IsKeyPointsValid => _tagsEditor.IsValid;

	IReadOnlyCollection<EditableTagDataContext> EditablePoserTagDataContext.KeyPointTags => KeyPointTags;
	IReadOnlyCollection<NewTagData> NewPoserTagData.KeyPointTags => _tagsEditor.Tags;

	public PoserNewTagViewModel(string name, IValidator<NewTagData> validator) : base(name, validator)
	{
	}

	public void Dispose()
	{
		_tagsEditor.Dispose();
	}

	private readonly TagsEditorViewModel _tagsEditor = new();
}