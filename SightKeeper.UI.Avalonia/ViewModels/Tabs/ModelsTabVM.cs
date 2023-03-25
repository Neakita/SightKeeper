using System;
using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;
using SightKeeper.UI.Avalonia.ViewModels.Elements;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabVM
{
	public ReadOnlyObservableCollection<ModelVM> Models => _models;
	
	public ModelVM? NewModel { get; private set; }


	public ModelsTabVM(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();

		_modelsSource.Connect()
			.Transform(ModelVM.Create)
			.Bind(out _models);
	}

	public ModelsTabVM() => throw new NotSupportedException();

	private readonly AppDbContextFactory _dbContextFactory;

	public void CreateModel()
	{
		NewModel = new DetectorModelVM(new DetectorModel("New model", new Resolution()));
	}

	public void ApplyModelCreation()
	{
		// using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		// dbContext.DetectorModels.Add(newModel);
		// dbContext.SaveChanges();
		// _modelsSource.AddOrUpdate(newModel);
	}

	// [DependsOn(nameof(NewModelName)), DependsOn(nameof(NewModelWidth)), DependsOn(nameof(NewModelHeight))]
	// private bool CanApplyModelCreation(object parameter) =>
	// 	!string.IsNullOrWhiteSpace(NewModelName) && NewModelWidth % 32 == 0 && NewModelHeight % 32 == 0;

	public void CancelModelCreation()
	{
		
	}

	public void DeleteModel(ModelVM modelVM)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Models.Remove(modelVM.Model);
		dbContext.SaveChanges();
		_modelsSource.Remove(modelVM.Model);
	}

	private readonly SourceCache<Model, int> _modelsSource = new(model => model.Id);
	private readonly ReadOnlyObservableCollection<ModelVM> _models;
}