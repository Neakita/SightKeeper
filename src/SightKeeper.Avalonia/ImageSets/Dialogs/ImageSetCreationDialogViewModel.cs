using System.ComponentModel;
using FluentValidation;
using SightKeeper.Application.ImageSets;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.ImageSets.Dialogs;

internal sealed class ImageSetCreationDialogViewModel : ImageSetDialogViewModel, ImageSetData
{
	public override string Header => "Create image set";

	public ImageSetCreationDialogViewModel(IValidator<ImageSetData> validator)
	{
		_viewModelValidator = new ViewModelValidator<ImageSetData>(validator, this, this);
		InitializeErrorHandling();
	}

	public override void Dispose()
	{
		base.Dispose();
		_viewModelValidator.Dispose();
	}

	protected override INotifyDataErrorInfo ErrorInfoSource => _viewModelValidator;

	private readonly ViewModelValidator<ImageSetData> _viewModelValidator;
}