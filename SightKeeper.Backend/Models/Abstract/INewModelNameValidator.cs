namespace SightKeeper.Backend.Models.Abstract;

public interface INewModelNameValidator
{
	bool IsValidName(string name, out string? validationMessage);
}