﻿using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbProfilesDataAccess : ProfilesDataAccess
{
    public IObservable<Profile> ProfileAdded => _profileAdded;
    public IObservable<Profile> ProfileRemoved => _profileRemoved;
    
    public DbProfilesDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task AddProfile(Profile profile, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.Profiles.Add(profile);
                _dbContext.SaveChanges();
            }
            _profileAdded.OnNext(profile);
        }, cancellationToken);
    }

    public Task RemoveProfile(Profile profile, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.Profiles.Remove(profile);
                _dbContext.SaveChanges();
            }
            _profileRemoved.OnNext(profile);
        }, cancellationToken);
    }

    public IObservable<Profile> LoadProfiles()
    {
        return Observable.Create<Profile>(observer =>
        {
            CancellationTokenSource cancellationTokenSource = new();
            Task.Run(() =>
            {
                LoadProfilesFromDb(cancellationTokenSource.Token).ToObservable().Subscribe(observer);
            }, cancellationTokenSource.Token);
            return Disposable.Create(() => cancellationTokenSource.Cancel());
        });
    }
    
    private readonly AppDbContext _dbContext;
    private readonly Subject<Profile> _profileAdded = new();
    private readonly Subject<Profile> _profileRemoved = new();

    private IEnumerable<Profile> LoadProfilesFromDb(CancellationToken cancellationToken)
    {
        var profileIndex = 0;
        while (true)
        {
            var profile = GetProfile(profileIndex);
            if (profile == null)
                break;
            Log.Debug("Loaded profile #{ProfileIndex} \"{Profile}\"", profileIndex, profile);
            yield return profile;
            cancellationToken.ThrowIfCancellationRequested();
            profileIndex++;
        }
    }

    private Profile? GetProfile(int index)
    {
        lock (_dbContext)
            return _dbContext.Profiles.Skip(index).FirstOrDefault();
    }
}