using FluentValidation;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application;

public sealed class ProfileCreator
{
    public ProfileCreator(IValidator<NewProfileData> validator, ProfilesDataAccess profilesDataAccess)
    {
        _validator = validator;
        _profilesDataAccess = profilesDataAccess;
    }
    
    public async Task<Profile> CreateProfile(NewProfileDataDTO data)
    {
        await _validator.ValidateAndThrowAsync(data);
        Profile profile = new(data.Name, data.Description, data.DetectionThreshold, data.MouseSensitivity, data.Weights);
        foreach (var itemClass in data.ItemClasses)
            profile.AddItemClass(itemClass);
        await _profilesDataAccess.AddProfile(profile);
        return profile;
    }
    
    private readonly IValidator<NewProfileData> _validator;
    private readonly ProfilesDataAccess _profilesDataAccess;
}