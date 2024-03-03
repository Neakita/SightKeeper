using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbProfilesDataAccess : ProfilesDataAccess
{
    public IObservable<Profile> ProfileAdded => _profileAdded;
    public IObservable<Profile> ProfileRemoved => _profileRemoved;
    public IObservable<Profile> ProfileUpdated => _profileUpdated;

    public DbProfilesDataAccess(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task AddProfile(Profile profile, CancellationToken cancellationToken)
    {
        await _dbContext.Profiles.AddAsync(profile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _profileAdded.OnNext(profile);
    }

    public async Task RemoveProfile(Profile profile, CancellationToken cancellationToken = default)
    {
        _dbContext.Profiles.Remove(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _profileRemoved.OnNext(profile);
    }

    public async Task UpdateProfile(Profile profile, CancellationToken cancellationToken = default)
    {
        _dbContext.Profiles.Update(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _profileUpdated.OnNext(profile);
    }

    public List<Profile> LoadProfiles()
    {
        return _dbContext.Profiles.ToList();
    }

    public Task<List<Profile>> LoadProfilesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Profiles.ToListAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Profile> _profileAdded = new();
    private readonly Subject<Profile> _profileUpdated = new();
    private readonly Subject<Profile> _profileRemoved = new();
}