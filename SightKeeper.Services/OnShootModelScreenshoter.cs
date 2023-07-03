using CommunityToolkit.Diagnostics;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Input;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Services;

public sealed class ShootModelScreenshoter : ModelScreenshoter
{
	public Model? Model
	{
		get => _model;
		set
		{
			if (IsEnabled)
				ThrowHelper.ThrowInvalidOperationException("Cannot change model while enabled");
			_model = value;
			_screenCapture.Resolution = _model?.Resolution;
			_screenCapture.Game = _model?.Game;
		}
	}

	public bool IsEnabled
	{
		get => _isEnabled;
		set
		{
			if (Model == null)
				ThrowHelper.ThrowInvalidOperationException("Cannot enable when no model selected");
			if (_isEnabled == value) return;
			_isEnabled = value;
			if (value)
				_hotKey = _hotKeyManager.Register(MouseButton.Button1, OnPressed);
			else _hotKey?.Dispose();
		}
	}

	public byte FramesPerSecond
	{
		get => _onHoldFPS;
		set
		{
			_onHoldFPS = value;
			_interval = 1000 / value;
		}
	}

	public ushort MaxImages { get; set; } = 500;

	public ShootModelScreenshoter(ScreenCapture screenCapture, HotKeyManager<MouseButton> hotKeyManager, AppDbContextFactory dbContextFactory)
	{
		_screenCapture = screenCapture;
		_hotKeyManager = hotKeyManager;
		_dbContextFactory = dbContextFactory;
		_interval = 1000 / FramesPerSecond;
	}

	private readonly ScreenCapture _screenCapture;
	private readonly HotKeyManager<MouseButton> _hotKeyManager;
	private readonly AppDbContextFactory _dbContextFactory;
	private Model? _model;
	private bool _isEnabled;
	private byte _onHoldFPS = 1;
	private int _interval;
	private bool _capturing;
	private HotKey? _hotKey;

	private void OnPressed(HotKey hotKey)
	{
		// TODO this is probably not thread safe
		Task.Run(() => OnPressedAsync(hotKey));
	}
	
	private async Task OnPressedAsync(HotKey hotKey, CancellationToken cancellationToken = default)
	{
		if (Model == null) return;
		if (!_screenCapture.CanCapture) return;
		if (_capturing) return; // TODO double checked locking?
		_capturing = true;
		while (hotKey.IsPressed)
		{
			await CaptureAsync(cancellationToken);
			Thread.Sleep(_interval);
		}
		await DeleteExceedScreenshotsAsync(cancellationToken);
		_capturing = false;
	}

	private async Task CaptureAsync(CancellationToken cancellationToken)
	{
		if (Model == null)
			ThrowHelper.ThrowInvalidOperationException("Cannot capture when no model selected");
		var image = await _screenCapture.CaptureAsync(cancellationToken);
		Screenshot screenshot = new(image);
		Model.AddScreenshot(screenshot);
	}

	private async Task DeleteExceedScreenshotsAsync(CancellationToken cancellationToken)
	{
		if (Model == null)
			ThrowHelper.ThrowInvalidOperationException("Cannot delete screenshots when no model selected");
		await Task.Run(() =>
		{
			var screenshotsToDelete = Model.Screenshots.OrderByDescending(screenshot => screenshot.CreationDate).Skip(MaxImages).ToList();
			foreach (var screenshot in screenshotsToDelete)
				Model.DeleteScreenshot(screenshot); // TODO optimization (delete by indexes)
		}, cancellationToken);
	}
}