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
using SightKeeper.Domain.Services;
using SightKeeper.Services.Input;

namespace SightKeeper.Services.Scoring;

public sealed class HotKeyProfileRunner : ProfileRunner
{
    public bool MakeScreenshots { get; set; }

    public float MinimumProbability
    {
        get => _minimumProbability;
        set
        {
            Guard.IsBetween(value, 0, 1);
            Guard.IsLessThanOrEqualTo(value, MaximumProbability);
            _minimumProbability = value;
            UpdateDetectionThreshold();
        }
    }

    public float MaximumProbability
    {
        get => _maximumProbability;
        set
        {
            Guard.IsBetween(value, 0, 1);
            Guard.IsGreaterThanOrEqualTo(value, MinimumProbability);
            _maximumProbability = value;
        }
    }

    public byte MaximumFPS
    {
        get => _maximumFPS;
        set
        {
            Guard.IsGreaterThan(value, (byte)0);
            _maximumFPS = value;
            _screenshotingDelay = TimeSpan.FromSeconds(1f / _maximumFPS);
        }
    }

    private TimeSpan _screenshotingDelay = TimeSpan.FromSeconds(1f / DefaultMaximumFPS);
    private DateTime _lastScreenshotTime = DateTime.UtcNow;

    private const byte DefaultMaximumFPS = 1;

    public HotKeyProfileRunner(
        StreamDetector streamDetector,
        MouseMover mouseMover,
        SharpHookHotKeyManager hotKeyManager,
        ProfileEditor profileEditor,
        ScreenshotsDataAccess screenshotsDataAccess)
    {
        _streamDetector = streamDetector;
        _mouseMover = mouseMover;
        _hotKeyManager = hotKeyManager;
        _profileEditor = profileEditor;
        _screenshotsDataAccess = screenshotsDataAccess;
        _streamDetector.ObservableDetection.Subscribe(t => HandleDetection(t.imageData, t.items)).DisposeWithEx(_constructorDisposables);
    }

    private bool _isFiring;
    
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
        _hotKeyManager.Register(MouseButton.Button4, () => _streamDetector.IsEnabled = true, () =>
            {
                _streamDetector.IsEnabled = false;
                Guard.IsNotNull(_currentProfile);
                _screenshotsDataAccess.SaveChanges(_currentProfile.Weights.Library.DataSet.ScreenshotsLibrary);
            })
            .DisposeWithEx(_currentProfileDisposables);
        _hotKeyManager.Register(MouseButton.Button1, () => _isFiring = true, () => _isFiring = false)
            .DisposeWithEx(_currentProfileDisposables);
        _profileEditor.ProfileEdited.Where(editedProfile => editedProfile == profile).Subscribe(OnProfileEdited)
            .DisposeWithEx(_currentProfileDisposables);
        UpdateDetectionThreshold();
    }

    private void OnProfileEdited(Profile profile)
    {
        _itemClassesIndexes = profile.ItemClasses.ToDictionary(
            profileItemClass => profileItemClass.ItemClass,
            profileItemClass => profileItemClass.Index);
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        UpdateDetectionThreshold();
    }

    private void HandleDetection(byte[] imageData, ImmutableList<DetectionItem> items)
    {
        Guard.IsNotNull(_itemClassesIndexes);
        Guard.IsNotNull(_currentProfile);
        var suitableItems = WhereSuitable(items).ToImmutableList();
        if (suitableItems.Any())
        {
            var mostSuitableItem = suitableItems.MinBy(GetItemOrder);
            MoveTo(mostSuitableItem.Bounding);
        }
        TryMakeScreenshot(imageData, items);
        if (_currentProfile.PostProcessDelay != TimeSpan.Zero)
        {
            if (_currentProfile.PostProcessDelay.TotalMilliseconds > 20)
                Thread.Sleep(_currentProfile.PostProcessDelay);
            else
                BurnTime(_currentProfile.PostProcessDelay);
        }
    }

    private IEnumerable<DetectionItem> WhereSuitable(IEnumerable<DetectionItem> items)
    {
        Guard.IsNotNull(_currentProfile);
        Guard.IsNotNull(_itemClassesIndexes);
        return items
            .Where(item => item.Probability >= _currentProfile.DetectionThreshold && _itemClassesIndexes.ContainsKey(item.ItemClass))
            .Where(IsFiringModePassing);
    }

    private bool IsFiringModePassing(DetectionItem item)
    {
        Guard.IsNotNull(_currentProfile);
        var profileItemClass =
            _currentProfile.ItemClasses.First(profileItemClass => profileItemClass.ItemClass == item.ItemClass);
        if (profileItemClass.ActivationCondition == ItemClassActivationCondition.None)
            return true;
        return (profileItemClass.ActivationCondition == ItemClassActivationCondition.IsShooting) == _isFiring;
    }

    private static void BurnTime(TimeSpan time)
    {
        var endTime = DateTime.UtcNow + time;
        while (DateTime.UtcNow < endTime) { }
    }

    private async void TryMakeScreenshot(byte[] imageData, ImmutableList<DetectionItem> items)
    {
        Guard.IsNotNull(_currentProfile);
        if (MakeScreenshots && !_isLoadingScreenshots && _lastScreenshotTime + _screenshotingDelay <= DateTime.UtcNow && items.Any(item => item.Probability >= MinimumProbability && item.Probability <= MaximumProbability))
        {
            if (_currentProfile.Weights.Library.DataSet.ScreenshotsLibrary.Screenshots == null)
            {
                _isLoadingScreenshots = true;
                await _screenshotsDataAccess.LoadAll(_currentProfile.Weights.Library.DataSet.ScreenshotsLibrary);
                _isLoadingScreenshots = false;
            }
            _lastScreenshotTime = DateTime.UtcNow;
            _currentProfile.Weights.Library.DataSet.ScreenshotsLibrary.CreateScreenshot(imageData);
        }
    }
    
    private bool _isLoadingScreenshots;

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
        moveVector = moveVector * _currentProfile.Weights.Library.DataSet.Resolution / 2;
        moveVector *= _currentProfile.MouseSensitivity;
        var moveX = (short)Math.Round(moveVector.X);
        var moveY = (short)Math.Round(moveVector.Y);
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
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;
    private Profile? _currentProfile;
    private CompositeDisposable? _currentProfileDisposables;
    private Dictionary<ItemClass, byte>? _itemClassesIndexes;
    private float _minimumProbability = 0.2f;
    private float _maximumProbability = 0.5f;
    private byte _maximumFPS = DefaultMaximumFPS;

    private void UpdateDetectionThreshold()
    {
        if (_currentProfile == null)
            return;
        _streamDetector.ProbabilityThreshold = Math.Min(MinimumProbability, _currentProfile.DetectionThreshold);
    }
}