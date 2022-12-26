namespace SightKeeper.Abstractions;

public interface INewModelNameValidator
{
	bool IsValidName(string name, out string? validationMessage);
}