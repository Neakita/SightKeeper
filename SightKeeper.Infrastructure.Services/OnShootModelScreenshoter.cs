using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;
using MouseButton = SharpHook.Native.MouseButton;

namespace SightKeeper.Infrastructure.Services;

public sealed class OnShootModelScreenshoter : ReactiveObject, ModelScreenshoter
{
	public Model? Model
	{
		get => _model;
		set
		{
			if (IsEnabled) throw new InvalidOperationException("Cannot change model when enabled");
			this.RaiseAndSetIfChanged(ref _model, value);
			_detectorModel = value as DetectorModel;
			if (_model == null) return;
			_ = LoadScreenshotsAsync(_model);
			_screenCapture.Resolution = _model.Resolution;
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
				_keyHook.MouseButtonPressed += KeyHookOnMouseButtonPressed;
			else
				_keyHook.MouseButtonPressed -= KeyHookOnMouseButtonPressed;
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

	[Reactive] public ushort MaxImages { get; set; } = 500;

	public OnShootModelScreenshoter(ScreenCapture screenCapture, KeyHook keyHook, AppDbContextFactory dbContextFactory)
	{
		_screenCapture = screenCapture;
		_keyHook = keyHook;
		_dbContextFactory = dbContextFactory;
		_interval = 1000 / FramesPerSecond;
	}

	private readonly ScreenCapture _screenCapture;
	private readonly KeyHook _keyHook;
	private readonly AppDbContextFactory _dbContextFactory;
	private Model? _model;
	private DetectorModel? _detectorModel;
	private bool _isEnabled;
	private byte _onHoldFPS = 1;
	private int _interval;
	private bool _capturing;

	private void KeyHookOnMouseButtonPressed(KeyHook sender, MouseButton button)
	{
		if (button != MouseButton.Button1) return;
		if (Model == null) return;
		if (_capturing) return;
		_detectorModel.ThrowIfNull(nameof(_detectorModel));
		_capturing = true;
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Attach(_detectorModel!);
		while (sender.IsPressed(button))
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
		DetectorScreenshot screenshot = new(_detectorModel, image);
		_detectorModel.DetectorScreenshots.Add(screenshot);
	}

	private void DeleteExceedScreenshots()
	{
		_detectorModel.ThrowIfNull(nameof(_detectorModel));
		List<int> notAssetsIndexes = _detectorModel!.DetectorScreenshots.Select((screenshot, index) => (screenshot, index)).Where(item => !item.screenshot.IsAsset).Select(item => item.index).ToList();
		if (notAssetsIndexes.Count <= MaxImages) return;
		IOrderedEnumerable<int> indexesToRemove = notAssetsIndexes.Take(notAssetsIndexes.Count - MaxImages).OrderDescending();
		foreach (int index in indexesToRemove)
			_detectorModel.DetectorScreenshots.RemoveAt(index);
	}

	private async Task LoadScreenshotsAsync(Model model)
	{
		if (model is DetectorModel detectorModel)
		{
			await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
			dbContext.Attach(model);
			await dbContext.Entry(detectorModel).Collection(m => m.DetectorScreenshots).LoadAsync();
			await dbContext.DetectorScreenshots.Where(screenshot => screenshot.Model == model).Select(screenshot => screenshot.Image).LoadAsync();
		}
		else throw new InvalidCastException();
	}
}