using FluentValidation;

namespace SightKeeper.Application.Games;

public sealed class GameDataValidator : AbstractValidator<GameData>
{
	public GameDataValidator()
	{
		RuleFor(game => game.Title).NotEmpty();
		RuleFor(game => game.ProcessName).NotEmpty();
		RuleFor(game => game.ExecutablePath).Must(ValidateExecutablePath).When(data => !string.IsNullOrEmpty(data.ExecutablePath));
	}

	private static bool ValidateExecutablePath(string path)
	{
		if (!Path.IsPathRooted(path))
			return false;
		try
		{
			FileInfo _ = new(path);
			return true;
		}
		catch
		{
			return false;
		}
	}
}