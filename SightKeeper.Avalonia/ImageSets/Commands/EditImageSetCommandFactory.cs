using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Commands;

internal sealed class EditImageSetCommandFactory
{
	public EditImageSetCommandFactory(
		ReadRepository<ImageSet> readImageSetsRepository,
		ImageSetEditor imageSetEditor,
		IValidator<ImageSetData> imageSetDataValidator,
		DialogManager dialogManager)
	{
		_readImageSetsRepository = readImageSetsRepository;
		_imageSetEditor = imageSetEditor;
		_imageSetDataValidator = imageSetDataValidator;
		_dialogManager = dialogManager;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<ImageSet>(EditImageSet!);
	}

	private readonly ReadRepository<ImageSet> _readImageSetsRepository;
	private readonly ImageSetEditor _imageSetEditor;
	private readonly IValidator<ImageSetData> _imageSetDataValidator;
	private readonly DialogManager _dialogManager;

	private async Task EditImageSet(ImageSet set)
	{
		var dialogHeader = $"Edit image set '{set.Name}'";
		IValidator<ImageSetData> validator = new ExistingImageSetDataValidator(
			set,
			_imageSetDataValidator,
			_readImageSetsRepository);
		using ImageSetDialogViewModel dialog = new(
			dialogHeader,
			validator,
			set.Name,
			set.Description);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetEditor.EditImageSet(set, dialog);
	}
}