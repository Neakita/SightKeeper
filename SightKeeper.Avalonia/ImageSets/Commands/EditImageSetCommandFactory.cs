using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ImageSets.Dialogs;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Commands;

internal sealed class EditImageSetCommandFactory
{
	public EditImageSetCommandFactory(
		ImageSetEditor imageSetEditor,
		DialogManager dialogManager,
		IValidator<ExistingImageSetData> validator)
	{
		_imageSetEditor = imageSetEditor;
		_dialogManager = dialogManager;
		_validator = validator;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<ImageSet>(EditImageSet!);
	}

	private readonly ImageSetEditor _imageSetEditor;
	private readonly DialogManager _dialogManager;
	private readonly IValidator<ExistingImageSetData> _validator;

	private async Task EditImageSet(ImageSet set)
	{
		using ImageSetEditingDialogViewModel dialog = new(set, _validator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetEditor.EditImageSet(dialog);
	}
}