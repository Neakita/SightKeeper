using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Tooling.Actions;
using SightKeeper.Avalonia.Annotation.Tooling.Adaptive;
using SightKeeper.Avalonia.Annotation.Tooling.DataSet;
using SightKeeper.Avalonia.Annotation.Tooling.ImageSet;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed partial class SideBarViewModel : ViewModel, SideBarDataContext, AdditionalToolingSelection, IDisposable
{
	public ImageSetSelectionDataContext ImageSetSelection { get; }
	public DataSetSelectionDataContext DataSetSelection { get; }
	public ActionsDataContext Actions { get; }
	[ObservableProperty] public partial object? AdditionalTooling { get; private set; }
	public IObservable<object?> AdditionalToolingChanged => _additionalToolingChanged.AsObservable();

	public SideBarViewModel(
		ImageSetSelectionDataContext imageSetSelection,
		DataSetSelectionDataContext dataSetSelectionDataContext,
		ActionsDataContext actions,
		DataSetSelection dataSetSelection,
		ToolingViewModelFactory toolingViewModelFactory)
	{
		ImageSetSelection = imageSetSelection;
		DataSetSelection = dataSetSelectionDataContext;
		Actions = actions;
		_toolingViewModelFactory = toolingViewModelFactory;
		_constructorDisposable = dataSetSelection.SelectedDataSetChanged.Subscribe(OnSelectedDataSetChanged);
	}

	public void Dispose()
	{
		_additionalToolingChanged.Dispose();
		_constructorDisposable.Dispose();
	}

	private void OnSelectedDataSetChanged(DataSet<Tag, Asset>? value)
	{
		AdditionalTooling = _toolingViewModelFactory.CreateToolingViewModel(value);
		_additionalToolingChanged.OnNext(AdditionalTooling);
	}

	private readonly Subject<object?> _additionalToolingChanged = new();
	private readonly ToolingViewModelFactory _toolingViewModelFactory;
	private readonly IDisposable _constructorDisposable;
}