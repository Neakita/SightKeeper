using System.Threading.Tasks;
using Material.Icons;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Data;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabVM : ViewModel
{
	public Repository<Model> ModelsRepository { get; }

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

	private async Task EditModel(Model model)
	{
		await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync();
		dbContext.Update(model);
		ModelEditorDialog editorDialog = new(model);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			await dbContext.SaveChangesAsync();
		else
			dbContext.RollBack();
	}

	private bool CanEditModel(object parameter) => parameter is Model;

	private async Task DeleteModel(Model model)
	{
		MessageBoxDialog.DialogResult result = await this.ShowMessageBoxDialog($"Do you really want to remove the {model.Name}? This action cannot be undone",
			MessageBoxDialog.DialogResult.Yes | MessageBoxDialog.DialogResult.No,
			"Confirm model deletion", MaterialIconKind.TrashCanOutline);
		if (result == MessageBoxDialog.DialogResult.Yes)
			ModelsRepository.Remove(model);
	}

	private bool CanDeleteModel(object parameter) => parameter is Model;
}