using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Backend.Models;

public sealed class DetectorModelsService : IModelsService<DetectorModel>
{
	private readonly IAppDbProvider _dbProvider;


	public DetectorModelsService(IAppDbProvider dbProvider) => _dbProvider = dbProvider;

	public DetectorModel Create(string name, ushort width, ushort height)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		DetectorModel newDetectorModel = new(name, new Resolution(width, height));
		dbContext.DetectorModels.Add(newDetectorModel);
		dbContext.SaveChanges();
		return newDetectorModel;
	}

	public void Delete(DetectorModel model)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.DetectorModels.Remove(model);
		dbContext.SaveChanges();
	}

	public void Delete(IEnumerable<DetectorModel> models)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.DetectorModels.RemoveRange(models);
		dbContext.SaveChanges();
	}
}