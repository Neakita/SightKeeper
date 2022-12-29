using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Backend.Models;

public sealed class GeneralModelsService : IModelsService<Model>
{
	public Model Create(string name, ushort width, ushort height)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		DetectorModel newDetectorModel = new(name, new Resolution(width, height));
		dbContext.DetectorModels.Add(newDetectorModel);
		dbContext.SaveChanges();
		return newDetectorModel;
	}

	public void Delete(Model model)
	{
		if (model is not DetectorModel detectorModel)
			throw new InvalidCastException($"Model {model} of type {model.GetType().FullName} cannot be casted to {typeof(DetectorModel).FullName} to delete it from db.");
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.DetectorModels.Remove(detectorModel);
		dbContext.SaveChanges();
	}

	public void Delete(IEnumerable<Model> models)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.DetectorModels.RemoveRange(models.Cast<DetectorModel>());
		dbContext.SaveChanges();
	}


	public GeneralModelsService(IAppDbProvider dbProvider) => _dbProvider = dbProvider;


	private readonly IAppDbProvider _dbProvider;
}
