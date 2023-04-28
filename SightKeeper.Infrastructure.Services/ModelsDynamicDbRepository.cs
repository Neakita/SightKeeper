using DynamicData;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Infrastructure.Services;

public sealed class ModelsDynamicDbRepository : GenericDynamicDbRepository<Model>
{
	public ModelsDynamicDbRepository(
		AppDbContextFactory dbContextFactory,
		Repository<Game> gamesRepository,
		Repository<ModelConfig> configsRepository) : base(dbContextFactory)
	{
		AppDbContext dbContext = dbContextFactory.CreateDbContext();
		var models = dbContext.Models
			.Select(model => new {modelId = model.Id, gameId = model.GameId, configId = model.ConfigId})
			.Where(modelInfo => modelInfo.gameId != null || modelInfo.configId != null)
			.ToList();
		foreach (var modelInfo in models)
		{
			Model model = Get(modelInfo.modelId);
			if (modelInfo.gameId != null)
				model.Game = gamesRepository.Get(modelInfo.gameId.Value);
			if (modelInfo.configId != null)
				model.Config = configsRepository.Get(modelInfo.configId.Value);
		}
	}

	public override void Add(Model item)
	{
		using AppDbContext dbContext = DbContextFactory.CreateDbContext();
		if (item.Game != null) dbContext.Attach(item.Game);
		DbSet<Model> set = dbContext.Models;
		set.Add(item);
		dbContext.SaveChanges();
		ItemsCache.AddOrUpdate(item);
	}
}
