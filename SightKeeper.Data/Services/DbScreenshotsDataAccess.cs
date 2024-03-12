using FlakeId;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
	public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

	public override Image LoadImage(Screenshot screenshot)
	{
		return _dbContext.Images.AsNoTracking().Single(image => EF.Property<Id>(image, "Id") == EF.Property<Id>(screenshot, "Id"));
	}
	public override IEnumerable<Image> LoadImages(IEnumerable<Screenshot> screenshots)
	{
		throw new NotImplementedException();
	}
	public override IEnumerable<Image> LoadImages(DataSet dataSet)
	{
		throw new NotImplementedException();
	}

	protected override void SaveScreenshot(Screenshot screenshot, Image image)
	{
	}

	private readonly AppDbContext _dbContext;
}