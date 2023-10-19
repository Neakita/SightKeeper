using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac;
using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Prediction;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Services.Input;
using SightKeeper.Services.Prediction.Handling;

namespace SightKeeper.Services.Prediction;

public sealed class HotKeyProfileRunner : ProfileRunner
{
    private sealed class Session
    {
        public CompositeDisposable Disposables { get; } = new();
        public Profile Profile { get; }
        public DetectionObserver Handler { get; set; }
        public ILifetimeScope Scope { get; set; }

        public Session(Profile profile, DetectionObserver handler, ILifetimeScope scope)
        {
            Profile = profile;
            Handler = handler;
            Scope = scope;
        }
    }

    public HotKeyProfileRunner(
        StreamDetector streamDetector,
        SharpHookHotKeyManager hotKeyManager,
        ProfileEditor profileEditor,
        ILifetimeScope scope)
    {
        _streamDetector = streamDetector;
        _hotKeyManager = hotKeyManager;
        _profileEditor = profileEditor;
        _scope = scope;
        _streamDetector.Detection.Subscribe(HandleDetection).DisposeWithEx(_constructorDisposables);
    }
    
    public void Run(Profile profile)
    {
        Guard.IsNull(_session);
        var scope = _scope.BeginLifetimeScope(profile, builder => BuildScope(builder, profile));
        _session = new Session(profile, ResolveObserver(scope), scope);
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        _hotKeyManager.Register(MouseButton.Button4, () => _streamDetector.IsEnabled = true, () =>
            {
                _streamDetector.IsEnabled = false;
                _session.Handler.OnPaused();
            })
            .DisposeWithEx(_session.Disposables);
        _profileEditor.ProfileEdited
            .Where(editedProfile => editedProfile == profile)
            .Subscribe(OnProfileEdited)
            .DisposeWithEx(_session.Disposables);

        _session.Handler.RequestedProbabilityThreshold.CombineLatest(_profileDetectionThreshold).Subscribe(t =>
        {
            var lowest = Math.Min(t.First ?? 1, t.Second ?? 1);
            _streamDetector.ProbabilityThreshold = lowest;
        });
        
        _profileDetectionThreshold.OnNext(profile.DetectionThreshold);
    }

    private void OnProfileEdited(Profile profile)
    {
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        _profileDetectionThreshold.OnNext(profile.DetectionThreshold);
        if (_session == null)
            return;
        _session.Scope.Dispose();
        _session.Scope = _scope.BeginLifetimeScope();
        _session.Handler = ResolveObserver(_session.Scope);
    }

    private static void BuildScope(ContainerBuilder builder, Profile profile)
    {
        builder.RegisterInstance(profile);
        builder.RegisterInstance(profile.Weights.Library.DataSet);
        builder.RegisterComposite<CompositeDetectionObserver, DetectionObserver>();
    }

    private static DetectionObserver ResolveObserver(IComponentContext scope)
    {
        return scope.Resolve<DetectionObserver>();
    }

    private void HandleDetection(DetectionData data)
    {
        Guard.IsNotNull(_session);
        _session.Handler.OnNext(data);
        var profile = _session.Profile;
        if (profile.PostProcessDelay != TimeSpan.Zero)
        {
            if (profile.PostProcessDelay.TotalMilliseconds > 20)
                Thread.Sleep(profile.PostProcessDelay);
            else
                BurnTime(profile.PostProcessDelay);
        }
    }

    private static void BurnTime(TimeSpan time)
    {
        var endTime = DateTime.UtcNow + time;
        while (DateTime.UtcNow < endTime) { }
    }

    public void Stop()
    {
        Guard.IsNotNull(_session);
        _session.Disposables.Dispose();
        _session.Scope.Dispose();
        _session = null;
        _streamDetector.IsEnabled = false;
        _streamDetector.Weights = null;
    }

    private readonly CompositeDisposable _constructorDisposables = new();
    private readonly StreamDetector _streamDetector;
    private readonly SharpHookHotKeyManager _hotKeyManager;
    private readonly ProfileEditor _profileEditor;
    private readonly ILifetimeScope _scope;
    private readonly BehaviorSubject<float?> _profileDetectionThreshold = new(null);
    private Session? _session;
}