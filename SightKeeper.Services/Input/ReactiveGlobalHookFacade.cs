using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SharpHook;
using SharpHook.Reactive;
using SightKeeper.Commons;

namespace SightKeeper.Services.Input;

public sealed class ReactiveGlobalHookFacade : IDisposable
{
	public bool IsRunning => _session != null;
	
	public IObservable<HookEventArgs> HookEnabled => _hookEnabled.AsObservable();
    private readonly Subject<HookEventArgs> _hookEnabled = new();
    
    public IObservable<HookEventArgs> HookDisabled => _hookDisabled.AsObservable();
    private readonly Subject<HookEventArgs> _hookDisabled = new();

    public IObservable<KeyboardHookEventArgs> KeyTyped => _keyTyped.AsObservable();
	private readonly ObservableSubject<KeyboardHookEventArgs> _keyTyped = new();

    public IObservable<KeyboardHookEventArgs> KeyPressed => _keyPressed.AsObservable();
	private readonly ObservableSubject<KeyboardHookEventArgs> _keyPressed = new();

    public IObservable<KeyboardHookEventArgs> KeyReleased => _keyReleased.AsObservable();
	private readonly ObservableSubject<KeyboardHookEventArgs> _keyReleased = new();

    public IObservable<MouseHookEventArgs> MouseClicked => _mouseClicked.AsObservable();
	private readonly ObservableSubject<MouseHookEventArgs> _mouseClicked = new();

    public IObservable<MouseHookEventArgs> MousePressed => _mousePressed.AsObservable();
	private readonly ObservableSubject<MouseHookEventArgs> _mousePressed = new();

    public IObservable<MouseHookEventArgs> MouseReleased => _mouseReleased.AsObservable();
	private readonly ObservableSubject<MouseHookEventArgs> _mouseReleased = new();

    public IObservable<MouseHookEventArgs> MouseMoved => _mouseMoved.AsObservable();
	private readonly ObservableSubject<MouseHookEventArgs> _mouseMoved = new();

    public IObservable<MouseHookEventArgs> MouseDragged => _mouseDragged.AsObservable();
	private readonly ObservableSubject<MouseHookEventArgs> _mouseDragged = new();

    public IObservable<MouseWheelHookEventArgs> MouseWheel => _mouseWheel.AsObservable();
	private readonly ObservableSubject<MouseWheelHookEventArgs> _mouseWheel = new();

    public ReactiveGlobalHookFacade()
    {
	    var hasObserversObservable = EnumerateSubjectsObserversObservable()
		    .Aggregate((x, y) => x.CombineLatest(y, (a, b) => a || b));
	    
	    _constructorDisposable = hasObserversObservable.DistinctUntilChanged().SkipWhile(hasObservers => !hasObservers).Subscribe(hasObservers =>
	    {
		    if (hasObservers)
			    RunAsync();
		    else
			    Stop();
	    });
    }

    public void Dispose()
    {
	    _hookEnabled.Dispose();
	    _hookDisabled.Dispose();
	    _keyTyped.Dispose();
	    _keyPressed.Dispose();
	    _keyReleased.Dispose();
	    _mouseClicked.Dispose();
	    _mousePressed.Dispose();
	    _mouseReleased.Dispose();
	    _mouseMoved.Dispose();
	    _mouseDragged.Dispose();
	    _mouseWheel.Dispose();
	    _constructorDisposable.Dispose();
	    _session?.Hook.Dispose();
    }

    private sealed record Session(SimpleReactiveGlobalHook Hook, IDisposable Disposable);

    private readonly IDisposable _constructorDisposable;
    private Session? _session;
    
    private void RunAsync()
    {
	    Guard.IsFalse(IsRunning);
	    var hook = new SimpleReactiveGlobalHook(true);
	    var disposable = SubscribeToEvents(hook);
	    _session = new Session(hook, disposable);
	    hook.RunAsync();
    }

    private void Stop()
    {
	    Guard.IsTrue(IsRunning);
	    Guard.IsNotNull(_session);
	    _session.Hook.Dispose();
	    _session.Disposable.Dispose();
	    _session = null;
    }

    private IEnumerable<IObservable<bool>> EnumerateSubjectsObserversObservable()
    {
	    yield return _keyTyped.HasObserversObservable;
	    yield return _keyPressed.HasObserversObservable;
	    yield return _keyReleased.HasObserversObservable;
	    yield return _mouseClicked.HasObserversObservable;
	    yield return _mousePressed.HasObserversObservable;
	    yield return _mouseReleased.HasObserversObservable;
	    yield return _mouseMoved.HasObserversObservable;
	    yield return _mouseDragged.HasObserversObservable;
	    yield return _mouseWheel.HasObserversObservable;
    }

    private CompositeDisposable SubscribeToEvents(SimpleReactiveGlobalHook hook)
    {
	    CompositeDisposable compositeDisposable = new();
	    hook.HookEnabled.Subscribe(_hookEnabled).DisposeWithEx(compositeDisposable);
	    hook.HookDisabled.Subscribe(_hookDisabled).DisposeWithEx(compositeDisposable);
	    hook.KeyTyped.Subscribe(_keyTyped).DisposeWithEx(compositeDisposable);
	    hook.KeyPressed.Subscribe(_keyPressed).DisposeWithEx(compositeDisposable);
	    hook.KeyReleased.Subscribe(_keyReleased).DisposeWithEx(compositeDisposable);
	    hook.MouseClicked.Subscribe(_mouseClicked).DisposeWithEx(compositeDisposable);
	    hook.MousePressed.Subscribe(_mousePressed).DisposeWithEx(compositeDisposable);
	    hook.MouseReleased.Subscribe(_mouseReleased).DisposeWithEx(compositeDisposable);
	    hook.MouseMoved.Subscribe(_mouseMoved).DisposeWithEx(compositeDisposable);
	    hook.MouseDragged.Subscribe(_mouseDragged).DisposeWithEx(compositeDisposable);
	    hook.MouseWheel.Subscribe(_mouseWheel).DisposeWithEx(compositeDisposable);
	    return compositeDisposable;
    }
}