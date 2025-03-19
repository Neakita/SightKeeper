using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Pure.DI;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.ImageSets;

internal sealed class CreateImageSetCommandFactory
{
	public CreateImageSetCommandFactory(
		[Tag("new")] IValidator<ImageSetData> newImageSetDataValidator,
		DialogManager dialogManager,
		ImageSetCreator imageSetCreator)
	{
		_newImageSetDataValidator = newImageSetDataValidator;
		_dialogManager = dialogManager;
		_imageSetCreator = imageSetCreator;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand(CreateImageSetAsync);
	}

	private readonly IValidator<ImageSetData> _newImageSetDataValidator;
	private readonly DialogManager _dialogManager;
	private readonly ImageSetCreator _imageSetCreator;

	private async Task CreateImageSetAsync()
	{
		using ImageSetDialogViewModel dialog = new("Create image set", _newImageSetDataValidator);
		if (await _dialogManager.ShowDialogAsync(dialog))
			_imageSetCreator.Create(dialog);
	}
}