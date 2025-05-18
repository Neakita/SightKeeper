using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.ImageSets.Commands;

internal sealed class CreateImageSetCommandFactory
{
	public CreateImageSetCommandFactory(
		IValidator<ImageSetData> validator,
		DialogManager dialogManager,
		ImageSetCreator imageSetCreator)
	{
		_validator = validator;
		_dialogManager = dialogManager;
		_imageSetCreator = imageSetCreator;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand(CreateImageSetAsync);
	}

	private readonly IValidator<ImageSetData> _validator;
	private readonly DialogManager _dialogManager;
	private readonly ImageSetCreator _imageSetCreator;

	private async Task CreateImageSetAsync()
	{
		using ImageSetCreationDialogViewModel creationDialog = new(_validator);
		if (await _dialogManager.ShowDialogAsync(creationDialog))
			_imageSetCreator.Create(creationDialog);
	}
}