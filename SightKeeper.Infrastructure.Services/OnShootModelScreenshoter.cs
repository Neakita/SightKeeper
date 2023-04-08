﻿using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
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
			LoadScreenshots(_model);
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

	public OnShootModelScreenshoter(ScreenCapture screenCapture, KeyHook keyHook, AppDbContextFactory dbContextFactory, ScreenBoundsProvider screenBoundsProvider)
	{
		_screenCapture = screenCapture;
		_keyHook = keyHook;
		_dbContextFactory = dbContextFactory;
		_screenBoundsProvider = screenBoundsProvider;
		_interval = 1000 / FramesPerSecond;
	}

	private readonly ScreenCapture _screenCapture;
	private readonly KeyHook _keyHook;
	private readonly AppDbContextFactory _dbContextFactory;
	private readonly ScreenBoundsProvider _screenBoundsProvider;
	private Model? _model;
	private DetectorModel? _detectorModel;
	private bool _isEnabled;
	private byte _onHoldFPS = 1;
	private int _interval;

	private void KeyHookOnMouseButtonPressed(KeyHook sender, MouseButton button)
	{
		if (button != MouseButton.Button1) return;
		if (Model == null) return;
		while (sender.IsPressed(button))
		{
			lock (this)
			{
				Capture();
			}
			Thread.Sleep(_interval);
		}
	}

	private void Capture()
	{
		if (_detectorModel == null) throw new InvalidOperationException("Detector model not set");
		byte[] bytes = _screenCapture.Capture();
		Image image = new(bytes);
		DetectorScreenshot screenshot = new(_detectorModel, image);
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Update(_detectorModel);
		_detectorModel.DetectorScreenshots.Add(screenshot);
		dbContext.SaveChanges();
		DeleteExceedScreenshots();
	}

	private void DeleteExceedScreenshots()
	{
		if (_detectorModel == null) throw new InvalidOperationException("Detector model is null");
		if (_detectorModel.DetectorScreenshots.Count > MaxImages)
		{
			int imagesToDelete = MaxImages - _detectorModel.DetectorScreenshots.Count;
			using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
			dbContext.Update(_detectorModel);
			for (int i = 0; i < imagesToDelete; i++)
				_detectorModel.DetectorScreenshots.RemoveAt(0);
			dbContext.SaveChanges();
		}
	}

	private void LoadScreenshots(Model model)
	{
		if (model is DetectorModel detectorModel)
		{
			using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
			dbContext.Update(model);
			dbContext.Entry(detectorModel).Collection(m => m.DetectorScreenshots).Load();
			dbContext.DetectorScreenshots.Where(screenshot => screenshot.Model == model).Select(screenshot => screenshot.Image).Load();
		}
		else throw new InvalidCastException();
	}
}
