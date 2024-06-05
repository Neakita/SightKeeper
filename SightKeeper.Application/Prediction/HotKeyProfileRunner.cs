using System.Reactive.Disposables;
using System.Reactive.Linq;
using Autofac;
using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Input;
using SightKeeper.Application.Prediction.Handling;
using SightKeeper.Application.Prediction.Handling.MouseMoving;
using SightKeeper.Application.Prediction.Handling.MouseMoving.Decorators;
using SightKeeper.Application.Prediction.Handling.MouseMoving.Decorators.Preemption;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Prediction;

public sealed class HotKeyProfileRunner : ProfileRunner
{
	private readonly ObjectsLookupper _objectsLookupper;

	private sealed class Session(Profile profile, DetectionObserver handler, ILifetimeScope scope)
    {
        public CompositeDisposable Disposables { get; } = new();
        public Profile Profile { get; } = profile;
        public DetectionObserver Handler { get; set; } = handler;
        public ILifetimeScope Scope { get; set; } = scope;
    }

	public HotKeyProfileRunner(
		StreamDetector streamDetector,
		SharpHookHotKeyManager hotKeyManager,
		ProfileEditor profileEditor,
		ILifetimeScope scope,
		ObjectsLookupper objectsLookupper)
	{
		_streamDetector = streamDetector;
		_hotKeyManager = hotKeyManager;
		_profileEditor = profileEditor;
		_scope = scope;
		_objectsLookupper = objectsLookupper;
	}

    public void Run(Profile profile)
    {
        Guard.IsNull(_session);
        var sessionScope = _scope.BeginLifetimeScope(profile, builder => BuildScope(builder, profile));
        _session = new Session(profile, ResolveObserver(sessionScope), sessionScope);
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        _hotKeyManager.Register(MouseButton.Button4, hotkey =>
            {
                var detectionDisposable = _streamDetector.RunObservable().Subscribe(HandleDetection);
                hotkey.WaitForRelease();
                detectionDisposable.Dispose();
                _session.Handler.OnPaused();
            })
            .DisposeWith(_session.Disposables);
        _profileEditor.ProfileEdited
            .Where(editedProfile => editedProfile == profile)
            .Subscribe(OnProfileEdited)
            .DisposeWith(_session.Disposables);

        _session.Handler.RequestedProbabilityThreshold.Subscribe(threshold =>
        {
            if (threshold != null)
                _streamDetector.ProbabilityThreshold = threshold.Value;
        });
    }

    private void OnProfileEdited(Profile profile)
    {
        _streamDetector.Weights = profile.Weights;
        _streamDetector.ProbabilityThreshold = profile.DetectionThreshold;
        if (_session == null)
            return;
        _session.Scope.Dispose();
        _session.Scope = _scope.BeginLifetimeScope(profile, builder => BuildScope(builder, profile));
        _session.Handler = ResolveObserver(_session.Scope);
    }

    private void BuildScope(ContainerBuilder builder, Profile profile)
    {
        builder.RegisterInstance(profile);
        var dataSet = _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(profile.Weights));
        builder.RegisterInstance(dataSet);
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
        _streamDetector.Weights = null;
    }

    private Session? _session;
    private readonly StreamDetector _streamDetector;
    private readonly SharpHookHotKeyManager _hotKeyManager;
    private readonly ProfileEditor _profileEditor;
    private readonly ILifetimeScope _scope;
}