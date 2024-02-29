using System.Reactive.Disposables;
using System.Reactive.Linq;
using Autofac;
using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Prediction;
using SightKeeper.Commons;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Services.Input;
using SightKeeper.Services.Prediction.Handling;
using SightKeeper.Services.Prediction.Handling.MouseMoving;
using SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators;
using SightKeeper.Services.Prediction.Handling.MouseMoving.Decorators.Preemption;

namespace SightKeeper.Services.Prediction;

public sealed class HotKeyProfileRunner(
        StreamDetector streamDetector,
        SharpHookHotKeyManager hotKeyManager,
        ProfileEditor profileEditor,
        ILifetimeScope scope)
    : ProfileRunner
{
    private sealed class Session(Profile profile, DetectionObserver handler, ILifetimeScope scope)
    {
        public CompositeDisposable Disposables { get; } = new();
        public Profile Profile { get; } = profile;
        public DetectionObserver Handler { get; set; } = handler;
        public ILifetimeScope Scope { get; set; } = scope;
    }

    public void Run(Profile profile)
    {
        Guard.IsNull(_session);
        var sessionScope = scope.BeginLifetimeScope(profile, builder => BuildScope(builder, profile));
        _session = new Session(profile, ResolveObserver(sessionScope), sessionScope);
        streamDetector.Weights = profile.Weights;
        streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        hotKeyManager.Register(MouseButton.Button4, hotkey =>
            {
                var detectionDisposable = streamDetector.RunObservable().Subscribe(HandleDetection);
                hotkey.WaitForRelease();
                detectionDisposable.Dispose();
                _session.Handler.OnPaused();
            })
            .DisposeWithEx(_session.Disposables);
        profileEditor.ProfileEdited
            .Where(editedProfile => editedProfile == profile)
            .Subscribe(OnProfileEdited)
            .DisposeWithEx(_session.Disposables);

        _session.Handler.RequestedProbabilityThreshold.Subscribe(threshold =>
        {
            if (threshold != null)
                streamDetector.ProbabilityThreshold = threshold.Value;
        });
    }

    private void OnProfileEdited(Profile profile)
    {
        streamDetector.Weights = profile.Weights;
        streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        if (_session == null)
            return;
        _session.Scope.Dispose();
        _session.Scope = scope.BeginLifetimeScope(profile, builder => BuildScope(builder, profile));
        _session.Handler = ResolveObserver(_session.Scope);
    }

    private static void BuildScope(ContainerBuilder builder, Profile profile)
    {
        builder.RegisterInstance(profile);
        builder.RegisterInstance(profile.Weights.Library.DataSet);
        if (profile.PreemptionSettings != null)
        {
            builder.RegisterDecorator<PreemptionDecorator, DetectionMouseMover>();
            if (profile.PreemptionSettings.StabilizationSettings == null)
                builder.RegisterType<SimplePreemptionComputer>().As<PreemptionComputer>();
            else
                builder.RegisterType<StabilizedPreemptionComputer>().As<PreemptionComputer>();
        }
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
        streamDetector.Weights = null;
    }

    private Session? _session;
}