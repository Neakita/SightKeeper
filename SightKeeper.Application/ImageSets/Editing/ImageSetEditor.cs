using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public class ImageSetEditor
{
	public IObservable<ImageSet> Edited => _edited;

	public ImageSetEditor(IValidator<ExistingImageSetData> validator)
	{
		_validator = validator;
	}

	public virtual void EditImageSet(ExistingImageSetData data)
	{
		_validator.Validate(data);
		var set = data.Set;
		set.Name = data.Name;
		set.Description = data.Description;
		_edited.OnNext(set);
	}

	private readonly IValidator<ExistingImageSetData> _validator;
	private readonly Subject<ImageSet> _edited = new();
}