using FluentValidation;
using SightKeeper.Application;

namespace SightKeeper.Data.Services;

public sealed class DbProfileEditor : ProfileEditor
{
	public DbProfileEditor(IValidator<EditedProfileData> validator, AppDbContext dbContext) : base(validator)
	{
		_dbContext = dbContext;
	}

	public override void ApplyChanges(EditedProfileDataDTO data)
	{
		base.ApplyChanges(data);
		for (var index = 0; index < data.Profile.Tags.Count; index++)
		{
			var profileTag = data.Profile.Tags[index];
			var tagEntry = _dbContext.Entry(profileTag);
			tagEntry.Property<byte>("Order").CurrentValue = (byte)index;
		}
	}

	private readonly AppDbContext _dbContext;
}