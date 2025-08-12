using System.ComponentModel;
using FluentValidation;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Dialogs;

internal sealed class ImageSetEditingDialogViewModel : ImageSetDialogViewModel, ExistingImageSetData
{
	public override string Header => $"Edit image set '{Set.Name}'";
	public ImageSet Set { get; }

	public ImageSetEditingDialogViewModel(ImageSet set, IValidator<ExistingImageSetData> validator)
	{
		Set = set;
		Name = set.Name;
		Description = set.Description;
		_viewModelValidator = new ViewModelValidator<ExistingImageSetData>(validator, this, this);
		InitializeErrorHandling();
	}

	public override void Dispose()
	{
		base.Dispose();
		_viewModelValidator.Dispose();
	}

	protected override INotifyDataErrorInfo ErrorInfoSource => _viewModelValidator;

	private readonly ViewModelValidator<ExistingImageSetData> _viewModelValidator;
}