/*using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Serilog;
using SharpHook.Native;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Input;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application.Prediction.Handling.MouseMoving;

public sealed class MouseMoverDetectionHandler : DetectionObserver, IDisposable
{
    public IObservable<float?> RequestedProbabilityThreshold { get; }
    
    public MouseMoverDetectionHandler(
	    Profile profile,
	    DetectionMouseMover mouseMover,
	    SharpHookHotKeyManager hotKeyManager,
	    ProfileEditor profileEditor)
    {
        _profile = profile;
        _mouseMover = mouseMover;
        _profileTags = profile.Tags.ToDictionary(
            profileTag => profileTag.Tag,
            profileTag => profileTag);
        _tagPriorities = profile.Tags.Select((profileTag, index) => (profileTag.Tag, index)).ToDictionary(
	        tuple => tuple.Tag,
	        tuple => tuple.index);
        if (profile.Tags.Any(tag => tag.ActivationCondition != ActivationCondition.None))
        {
            hotKeyManager.Register(MouseButton.Button1, 
                    () => _isFiring = true,
                    () => _isFiring = false)
                .DisposeWith(_constructorDisposables);
        }
        
        RequestedProbabilityThreshold = profileEditor.ProfileEdited
            .Where(editedProfile => editedProfile == profile)
            .Select(editedProfile => (float?)editedProfile.DetectionThreshold);
    }

    public void OnNext(DetectionData data)
    {
        if (_disposed)
            return;
        var suitableItems = WhereSuitable(data.Items).ToImmutableList();
        if (!suitableItems.Any())
            return;
        var mostSuitableItem = suitableItems.MinBy(GetItemOrder);
        MoveTo(data, mostSuitableItem);
    }

    public void OnPaused()
    {
        _mouseMover.OnPaused();
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _constructorDisposables.Dispose();
        _disposed = true;
    }

    private readonly Profile _profile;
    private readonly Dictionary<Tag, ProfileTag> _profileTags;
    private readonly Dictionary<Tag, int> _tagPriorities;
    private readonly DetectionMouseMover _mouseMover;
    private readonly ObjectsLookupper _objectsLookupper;
    private readonly CompositeDisposable _constructorDisposables = new();
    private bool _isFiring;
    private bool _disposed;
    
    private IEnumerable<DetectionItem> WhereSuitable(IEnumerable<DetectionItem> items)
    {
        return items
            .Where(item => item.Probability >= _profile.DetectionThreshold && _profileTags.ContainsKey(item.Tag))
            .Where(IsFiringModePassing);
    }
    
    private bool IsFiringModePassing(DetectionItem item)
    {
        var profileTag = _profileTags[item.Tag];
        if (profileTag.ActivationCondition == ActivationCondition.None)
            return true;
        return (profileTag.ActivationCondition == ActivationCondition.IsShooting) == _isFiring;
    }
    
    private float GetItemOrder(DetectionItem item)
    {
        var distance = GetNormalizedDistanceTo(item.Bounding);
        var tagIndex = _tagPriorities[item.Tag];
        var order = distance + tagIndex;
        Log.Debug("Order of item {Item} is {Order} (Distance: {Distance}, Item class index: {TagIndex})",
            item, order, distance, tagIndex);
        return order;
    }
    
    private static float GetNormalizedDistanceTo(RectangleF rectangle) => GetMoveVector(rectangle).Length();

    private void MoveTo(DetectionData data, DetectionItem item)
    {
        var moveVector = GetMoveVector(item.Bounding);
        // TODO optimize (do not use GetDataSet & GetLibrary every call)
        moveVector = moveVector * _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(_profile.Weights)).Resolution / 2;
        moveVector *= _profile.MouseSensitivity;
        _mouseMover.Move(new MouseMovingContext(data, item), moveVector);
    }

    private static Vector2 GetMoveVector(RectangleF rectangle)
    {
        var center = new Vector2(0.5f, 0.5f);
        var pos = new Vector2((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);
        var difference = pos - center;
        return difference;
    }
}*/