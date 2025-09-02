using FluentValidation;

namespace SightKeeper.Application.ImageSets.Editing;

public sealed class ImageSetEditor
{
	public ImageSetEditor(IValidator<ExistingImageSetData> validator)
	{
		_validator = validator;
	}

	public void EditImageSet(ExistingImageSetData data)
	{
		_validator.Validate(data);
		var set = data.Set;
		set.Name = data.Name;
		set.Description = data.Description;
	}

	private readonly IValidator<ExistingImageSetData> _validator;
}