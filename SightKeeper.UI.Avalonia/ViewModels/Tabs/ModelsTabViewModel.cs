using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DynamicData;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Data;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.ViewModels.Elements;
using ModelEditor = SightKeeper.UI.Avalonia.Views.Windows.ModelEditor;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<ModelViewModel> Models => _models;

	public ModelsTabViewModel(AppDbContextFactory dbContextFactory) : this()
	{
		using AppDbContext dbContext = dbContextFactory.CreateDbContext();
		_modelsSource.AddOrUpdate(dbContext.Models);
	}

	public ModelsTabViewModel()
	{
		_modelsSource.Connect()
			.Transform(ModelViewModel.Create)
			.Bind(out _models)
			.Subscribe();
	}

	private readonly SourceCache<Model, int> _modelsSource = new(model => model.Id);
	private readonly ReadOnlyObservableCollection<ModelViewModel> _models;

	private async Task CreateNewModel()
	{
		ModelViewModel model = ModelViewModel.Create(new DetectorModel("Unnamed detector model"));
		ModelEditor editor = new(model);
		ModelEditor.DialogResult result = await this.ShowDialog(editor);
		if (result == ModelEditor.DialogResult.Apply) _modelsSource.AddOrUpdate(model.Model);
	}
}