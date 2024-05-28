using FluentValidation;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public sealed class GameValidator : AbstractValidator<Game>
{
	public GameValidator()
	{
		RuleFor(game => game.Title).NotEmpty();
		RuleFor(game => game.ProcessName).NotEmpty();
	}
}