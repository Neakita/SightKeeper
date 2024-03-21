using System.Collections.Immutable;
using FlakeId;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Services;

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
	public override IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IEnumerable<Screenshot> screenshots, bool ordered = false)
	{
		return LoadImages(screenshots as IReadOnlyCollection<Screenshot> ?? screenshots.ToImmutableList(), ordered);
	}
	public override IEnumerable<(Screenshot screenshot, Image image)> LoadImages(IReadOnlyCollection<Screenshot> screenshots, bool ordered = false)
	{
		var unsavedMatchingImages = screenshots.Where(screenshot => _unsavedImages.ContainsKey(screenshot))
			.Select(screenshot => (screenshot, _unsavedImages[screenshot]));
		var screenshotIds = screenshots.Select(screenshot => _dbContext.Entry(screenshot).Property<Id>("Id").CurrentValue).ToList();
		var screenshotsLookup = screenshots.ToDictionary(screenshot => _dbContext.Entry(screenshot).Property<Id>("Id").CurrentValue);
		var query = _dbContext
			.Images
			.AsNoTracking()
			.Select(image => new
			{
				image,
				id = EF.Property<Id>(image, "Id")
			})
			.Where(tuple => screenshotIds.Contains(tuple.id))
			.AsEnumerable()
			.Select(tuple => (screenshot: screenshotsLookup[tuple.id], tuple.image))
			.Concat(unsavedMatchingImages);
		if (!ordered)
			return query;
		var orderLookup = screenshots.Select((screenshot, index) => (screenshot, index))
				.ToDictionary(tuple => tuple.screenshot, tuple => tuple.index);
		query = query.OrderBy(tuple => orderLookup[tuple.screenshot]);
		return query;
	}
	public override IEnumerable<(Screenshot screenshot, Image image)> LoadImages(DataSet dataSet)
	{
		var unsavedMatchingImages = _unsavedImages
			.Where(pair => pair.Key.Library == dataSet.Screenshots)
			.Select(pair => (pair.Key, pair.Value));
		var query = _dbContext
			.Entry(dataSet.Screenshots)
			.Collection<Screenshot>("_screenshots")
			.Query()
			.Select(screenshot => new
			{
				screenshot,
				image = _dbContext.Images.Single(image => EF.Property<Id>(image, "Id") == EF.Property<Id>(screenshot, "Id"))
			})
			.AsEnumerable()
			.Select(pair => (pair.screenshot, pair.image))
			.Concat(unsavedMatchingImages);
		return query;
	}

	public override void Dispose()
	{
		base.Dispose();
		_dbContext.SavedChanges -= OnDbContextSavedChanges;
	}

	protected override void SaveScreenshot(Screenshot screenshot, Image image)
	{
		var id = Id.Create();
		_dbContext.Add(screenshot).SetId(id);
		_dbContext.Add(image).SetId(id);
		_unsavedImages.Add(screenshot, image);
	}

	private readonly AppDbContext _dbContext;
	private readonly Dictionary<Screenshot, Image> _unsavedImages = new();

	private void OnDbContextSavedChanges(object? sender, SavedChangesEventArgs e)
	{
		_unsavedImages.Clear();
	}
}