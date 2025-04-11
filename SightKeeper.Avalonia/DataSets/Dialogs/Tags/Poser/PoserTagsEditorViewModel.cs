using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Avalonia.DataSets.Dialogs.Tags.Poser;

internal sealed class PoserTagsEditorViewModel : TagsEditorViewModel
{
	public PoserTagsEditorViewModel()
	{
	}

	public PoserTagsEditorViewModel(IEnumerable<PoserTag> existingTags)
	{
		var tagsToAdd = existingTags.Select(tag => new EditedPoserTagViewModel(Validator, tag));
		AddTags(tagsToAdd);
	}

	protected override TagDataViewModel CreateTagViewModel(string name, NewTagDataIndividualValidator validator)
	{
		PoserNewTagViewModel newTag = new(name, validator);
		newTag.IsKeyPointsValid.Subscribe(_ => UpdateIsValid()).DisposeWith(_disposable);
		return newTag;
	}

	protected override bool IsTagValid(TagDataViewModel newTag)
	{
		return base.IsTagValid(newTag) && ((PoserNewTagViewModel)newTag).IsKeyPointsValid;
	}

	private readonly CompositeDisposable _disposable = new();
}