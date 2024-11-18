using FluentValidation;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Games;

public sealed class GameCreator
{
	public GameCreator(IValidator<GameData> validator, WriteDataAccess<Game> dataAccess)
	{
		_validator = validator;
		_dataAccess = dataAccess;
	}

	public Game CreateGame(GameData data)
	{
		_validator.ValidateAndThrow(data);
		Game game = new(data.Title, data.ProcessName, data.ExecutablePath);
		_dataAccess.Add(game);
		return game;
	}

	private readonly IValidator<GameData> _validator;
	private readonly WriteDataAccess<Game> _dataAccess;
}