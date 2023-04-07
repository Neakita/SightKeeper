using System;
using System.Threading.Tasks;
using Avalonia.Metadata;
using Material.Icons;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabVM : ViewModel
{
	public static ModelsTabVM New => Locator.Resolve<ModelsTabVM>();


	public Repository<Model> ModelsRepository { get; }
	[Reactive] public Model? SelectedModel { get; set; }

	public ModelsTabVM(Repository<Model> modelsRepository, AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
		ModelsRepository = modelsRepository;
	}
	
	private readonly AppDbContextFactory _dbContextFactory;

	private async Task CreateNewModel()
	{
		DetectorModel newModel = new("Unnamed detector model");
		ModelEditorDialog editorDialog = new(newModel);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			ModelsRepository.Add(newModel);
	}

	public async Task EditModel()
	{
		if (SelectedModel == null) throw new Exception();
		await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
		dbContext.Update(SelectedModel);
		ModelEditorDialog editorDialog = new(SelectedModel);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			await dbContext.SaveChangesAsync();
		else
			dbContext.RollBack();
	}

	[DependsOn(nameof(SelectedModel))]
	public bool CanEditModel(object parameter) => SelectedModel != null;

	public async Task DeleteModel()
	{
		if (SelectedModel == null) throw new Exception();
		MessageBoxDialog.DialogResult result = await this.ShowMessageBoxDialog($"Do you really want to remove the {SelectedModel.Name}? This action cannot be undone",
			MessageBoxDialog.DialogResult.Yes | MessageBoxDialog.DialogResult.No,
			"Confirm model deletion", MaterialIconKind.TrashCanOutline);
		if (result == MessageBoxDialog.DialogResult.Yes)
			ModelsRepository.Remove(SelectedModel);
	}

	[DependsOn(nameof(SelectedModel))]
	public bool CanDeleteModel(object parameter) => SelectedModel != null;
}