using ReactiveUI;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Input;
using SightKeeper.Common;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using Image = SightKeeper.Domain.Model.Common.Image;
using MouseButton = SharpHook.Native.MouseButton;

namespace SightKeeper.Services;

public sealed class ShootModelScreenshoter : ModelScreenshoter
{
	public Model? Model
	{
		get => _model;
		set
		{
			if (IsEnabled) throw new InvalidOperationException("Cannot change model while enabled");
			_detectorModel = value as DetectorModel;
			if (_model == null) return;
			_ = LoadScreenshotsAsync(_model);
			_screenCapture.Resolution = _model.Resolution;
			_screenCapture.Game = _model.Game;
		}
	}

	public bool IsEnabled
	{
		get => _isEnabled;
		set
		{
			if (Model == null) throw new InvalidOperationException("Cannot enable when no model selected");
			this.RaiseAndSetIfChanged(ref _isEnabled, value);
			if (value)
				_hotKey = _hotKeyManager.Register(MouseButton.Button1, Pressed);
			else _hotKey.Dispose();
		}
	}

	public byte FramesPerSecond
	{
		get => _onHoldFPS;
		set
		{
			if (_onHoldFPS == value) return;
			this.RaiseAndSetIfChanged(ref _onHoldFPS, value);
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
	private DetectorModel? _detectorModel;
	private bool _isEnabled;
	private byte _onHoldFPS = 1;
	private int _interval;
	private bool _capturing;
	private HotKey _hotKey;

	private void Pressed(HotKey hotKey)
	{
		if (Model == null) return;
		if (!_screenCapture.CanCapture) return;
		if (_capturing) return;
		_detectorModel.ThrowIfNull(nameof(_detectorModel));
		_capturing = true;
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Attach(_detectorModel!);
		while (hotKey.IsPressed)
		{
			lock (this)
			{
				Capture();
			}
			Thread.Sleep(_interval);
		}
		DeleteExceedScreenshots();
		dbContext.SaveChanges();
		_capturing = false;
	}

	private void Capture()
	{
		if (_detectorModel == null) throw new InvalidOperationException("Detector model not set");
		byte[] bytes = _screenCapture.Capture();
		Image image = new(bytes);
		DetectorAsset asset = new(image);
		_detectorModel.Screenshots.Add(asset);
	}

	private void DeleteExceedScreenshots()
	{
		_detectorModel.ThrowIfNull(nameof(_detectorModel));
		List<int> notAssetsIndexes = _detectorModel!.Screenshots.Select((screenshot, index) => (screenshot, index)).Where(item => !item.screenshot.IsAsset).Select(item => item.index).ToList();
		if (notAssetsIndexes.Count <= MaxImages) return;
		IOrderedEnumerable<int> indexesToRemove = notAssetsIndexes.Take(notAssetsIndexes.Count - MaxImages).OrderDescending();
		foreach (int index in indexesToRemove)
			_detectorModel.Screenshots.RemoveAt(index);
	}

	private async Task LoadScreenshotsAsync(Model model)
	{
		if (model is DetectorModel detectorModel)
		{
			await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
			dbContext.Attach(model);
			await dbContext.Entry(detectorModel).Collection(m => m.Screenshots).LoadAsync();
		}
		else throw new InvalidCastException();
	}
}