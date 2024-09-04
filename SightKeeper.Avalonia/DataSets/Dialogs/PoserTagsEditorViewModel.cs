using System;
using System.Reactive.Disposables;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

internal sealed class PoserTagsEditorViewModel : TagsEditorViewModel
{
	protected override TagViewModel CreateTagViewModel(string name, TagDataValidator validator)
	{
		PoserTagViewModel tag = new(name, validator);
		tag.IsKeyPointsValid.Subscribe(_ => UpdateIsValid()).DisposeWith(_disposable);
		return tag;
	}

	protected override bool IsTagValid(TagViewModel tag)
	{
		return base.IsTagValid(tag) && ((PoserTagViewModel)tag).IsKeyPointsValid;
	}

	private readonly CompositeDisposable _disposable = new();
}