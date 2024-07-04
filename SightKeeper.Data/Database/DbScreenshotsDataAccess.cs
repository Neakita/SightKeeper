using FlakeId;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Database;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
	public DbScreenshotsDataAccess(AppDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbContext.SavedChanges += OnDbContextSavedChanges;
	}

	public override Image LoadImage(Screenshot screenshot)
	{
		if (_unsavedImages.TryGetValue(screenshot, out var unsavedImage))
			return unsavedImage;
		return _dbContext.Images.AsNoTracking().Single(image => EF.Property<Id>(image, "Id") == _dbContext.Entry(screenshot).Property<Id>("Id").CurrentValue);
	}

	public void Dispose()
	{
		_dbContext.SavedChanges -= OnDbContextSavedChanges;
	}

	protected override void SaveScreenshotData(Screenshot screenshot, Image image)
	{
		var id = Id.Create();
		_dbContext.Entry(screenshot).SetId(id);
		_dbContext.Add(image).SetId(id);
		_unsavedImages.Add(screenshot, image);
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_dbContext.Remove(screenshot);
		_unsavedImages.Remove(screenshot);
	}

	private readonly AppDbContext _dbContext;
	private readonly Dictionary<Screenshot, Image> _unsavedImages = new();

	private void OnDbContextSavedChanges(object? sender, SavedChangesEventArgs e)
	{
		DetachImages();
		_unsavedImages.Clear();
	}

	private void DetachImages()
	{
		foreach (var image in _unsavedImages.Values)
			_dbContext.Entry(image).State = EntityState.Detached;
	}
}