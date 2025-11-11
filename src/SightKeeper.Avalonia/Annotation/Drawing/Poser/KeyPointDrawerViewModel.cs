using System;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Annotation.Tooling.Tags;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

public sealed partial class KeyPointDrawerViewModel : ViewModel, KeyPointDrawerDataContext, IDisposable
{
	[ObservableProperty, NotifyPropertyChangedFor(nameof(ExistingKeyPoint)), NotifyCanExecuteChangedFor(nameof(CreateKeyPointCommand))]
	public partial PoserItemViewModel? Item { get; set; }

	public KeyPointViewModel? ExistingKeyPoint => Item?.KeyPoints.SingleOrDefault(keyPoint => keyPoint.Tag == _tag);

	ICommand KeyPointDrawerDataContext.CreateKeyPointCommand => CreateKeyPointCommand;

	public KeyPointDrawerViewModel(TagSelectionProvider tagSelectionProvider)
	{
		_constructorDisposable = tagSelectionProvider.SelectedTagChanged.Subscribe(HandleTagSelectionChange);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private readonly IDisposable _constructorDisposable;
	private Tag? _tag;
	private bool CanCreateItem => _tag != null && Item != null;

	[RelayCommand(CanExecute = nameof(CanCreateItem))]
	private void CreateKeyPoint(Vector2<double> position)
	{
		Guard.IsNotNull(_tag);
		Guard.IsNotNull(Item);
		var keyPoint = Item.Value.MakeKeyPoint(_tag);
		keyPoint.Position = position;
	}

	private void HandleTagSelectionChange(Tag? tag)
	{
		if (tag == null || !tag.IsKeyPointTag())
			_tag = null;
		else
			_tag = tag;
		OnPropertyChanged(nameof(ExistingKeyPoint));
		CreateKeyPointCommand.NotifyCanExecuteChanged();
	}
}