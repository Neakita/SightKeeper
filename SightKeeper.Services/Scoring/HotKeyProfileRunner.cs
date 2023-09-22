using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Diagnostics;
using Serilog;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Scoring;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Services.Input;

namespace SightKeeper.Services.Scoring;

public sealed class HotKeyProfileRunner : ProfileRunner
{
    public HotKeyProfileRunner(StreamDetector streamDetector, MouseMover mouseMover, SharpHookHotKeyManager hotKeyManager, ProfileEditor profileEditor)
    {
        _streamDetector = streamDetector;
        _mouseMover = mouseMover;
        _hotKeyManager = hotKeyManager;
        _profileEditor = profileEditor;
        _streamDetector.ObservableDetection.Subscribe(HandleDetection).DisposeWithEx(_constructorDisposables);
    }
    
    public void Run(Profile profile)
    {
        Guard.IsNull(_currentProfile);
        Guard.IsNull(_currentProfileDisposables);
        _currentProfile = profile;
        _itemClassesIndexes = profile.ItemClasses.ToDictionary(
            profileItemClass => profileItemClass.ItemClass,
            profileItemClass => profileItemClass.Index);
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        _currentProfileDisposables = new CompositeDisposable();
        _hotKeyManager.Register(MouseButton.Button4, () => _streamDetector.IsEnabled = true, () => _streamDetector.IsEnabled = false)
            .DisposeWithEx(_currentProfileDisposables);
        _profileEditor.ProfileEdited.Where(editedProfile => editedProfile == profile).Subscribe(OnProfileEdited)
            .DisposeWithEx(_currentProfileDisposables);
    }

    private void OnProfileEdited(Profile profile)
    {
        _itemClassesIndexes = profile.ItemClasses.ToDictionary(
            profileItemClass => profileItemClass.ItemClass,
            profileItemClass => profileItemClass.Index);
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
    }

    private void HandleDetection(ImmutableList<DetectionItem> items)
    {
        Guard.IsNotNull(_itemClassesIndexes);
        var suitableItems = items.Where(item => _itemClassesIndexes.ContainsKey(item.ItemClass)).ToImmutableList();
        if (!suitableItems.Any())
            return;
        var item = suitableItems.MinBy(GetItemOrder);
        MoveTo(item.Bounding);
    }

    private float GetItemOrder(DetectionItem item)
    {
        Guard.IsNotNull(_itemClassesIndexes);
        var distance = GetNormalizedDistanceTo(item.Bounding);
        var itemClassIndex = _itemClassesIndexes[item.ItemClass];
        var order = distance + itemClassIndex;
        Log.Debug("Order of item {Item} is {Order} (Distance: {Distance}, Item class index: {ItemClassIndex})",
            item, order, distance, itemClassIndex);
        return order;
    }

    private static float GetNormalizedDistanceTo(RectangleF rectangle) => GetMoveVector(rectangle).Length();

    private void MoveTo(RectangleF rectangle)
    {
        Guard.IsNotNull(_currentProfile);
        var moveVector = GetMoveVector(rectangle);
        var scaledMoveVector = moveVector * _currentProfile.Weights.Library.DataSet.Resolution / 2;
        var scaledMoveVectorWithSensitivityFactor = scaledMoveVector * _currentProfile.MouseSensitivity;
        var moveX = (short)Math.Round(scaledMoveVectorWithSensitivityFactor.X);
        var moveY = (short)Math.Round(scaledMoveVectorWithSensitivityFactor.Y);
        _mouseMover.Move(moveX, moveY);
    }

    private static Vector2 GetMoveVector(RectangleF rectangle)
    {
        var center = new Vector2(0.5f, 0.5f);
        var pos = new Vector2((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);
        var difference = pos - center;
        return difference;
    }

    public void Stop()
    {
        Guard.IsNotNull(_currentProfile);
        Guard.IsNotNull(_currentProfileDisposables);
        _currentProfile = null;
        _currentProfileDisposables.Dispose();
        _currentProfileDisposables = null;
        _streamDetector.IsEnabled = false;
        _streamDetector.Weights = null;
    }

    private readonly CompositeDisposable _constructorDisposables = new();
    private readonly StreamDetector _streamDetector;
    private readonly MouseMover _mouseMover;
    private readonly SharpHookHotKeyManager _hotKeyManager;
    private readonly ProfileEditor _profileEditor;
    private Profile? _currentProfile;
    private CompositeDisposable? _currentProfileDisposables;
    private Dictionary<ItemClass, byte>? _itemClassesIndexes;
}