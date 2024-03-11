using FluentValidation;

namespace SightKeeper.Application;

public sealed class NewProfileDataValidator : AbstractValidator<NewProfileData>
{
    private readonly ProfilesDataAccess _profilesDataAccess;

    public NewProfileDataValidator(IValidator<ProfileData> profileDataValidator, ProfilesDataAccess profilesDataAccess)
    {
        _profilesDataAccess = profilesDataAccess;
        Include(profileDataValidator);
        RuleFor(data => data.Name)
            .Must(IsNameFree);
    }

    private bool IsNameFree(string name)
    {
        return _profilesDataAccess.Profiles.All(profile => profile.Name != name);
    }
}