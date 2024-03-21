using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Services;

public sealed class DbProfilesDataAccess : ProfilesDataAccess
{
    public IObservable<Profile> ProfileAdded => _profileAdded;
    public IObservable<Profile> ProfileRemoved => _profileRemoved;
    public IReadOnlyCollection<Profile> Profiles => _profiles;

    public DbProfilesDataAccess(AppDbContext dbContext)
    {
	    _profiles = new HashSet<Profile>(dbContext.Profiles.Include(profile => profile.ItemClasses.OrderBy(itemClass => EF.Property<byte>(itemClass, "Order"))));
	    _dbContext = dbContext;
    }

    public void AddProfile(Profile profile)
    {
	    bool isAdded = _profiles.Add(profile);
	    Guard.IsTrue(isAdded);
        _dbContext.Profiles.Add(profile);
        _profileAdded.OnNext(profile);
    }

    public void RemoveProfile(Profile profile)
    {
	    bool isRemoved = _profiles.Remove(profile);
	    Guard.IsTrue(isRemoved);
        _dbContext.Profiles.Remove(profile);
        _profileRemoved.OnNext(profile);
    }

    private readonly HashSet<Profile> _profiles;
    private readonly AppDbContext _dbContext;
    private readonly Subject<Profile> _profileAdded = new();
    private readonly Subject<Profile> _profileRemoved = new();
}