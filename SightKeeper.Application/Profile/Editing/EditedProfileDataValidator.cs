using FluentValidation;

namespace SightKeeper.Application;

public sealed class EditedProfileDataValidator : AbstractValidator<EditedProfileData>
{
    public EditedProfileDataValidator(IValidator<ProfileData> profileDataValidator, ProfilesDataAccess profilesDataAccess)
    {
        _profilesDataAccess = profilesDataAccess;
        Include(profileDataValidator);
        RuleFor(data => data.Profile).NotNull();
        RuleFor(data => data.Name)
            .Must((data, _) => IsNameFree(data));
    }

    private bool IsNameFree(EditedProfileData data)
    {
        return _profilesDataAccess.Profiles.Where(profile => profile != data.Profile).All(profile => profile.Name != data.Name);
    }
    
    private readonly ProfilesDataAccess _profilesDataAccess;
}