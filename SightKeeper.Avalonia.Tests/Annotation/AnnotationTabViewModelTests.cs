using System.Reactive.Subjects;
using NSubstitute;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;

namespace SightKeeper.Avalonia.Tests.Annotation;

public sealed class AnnotationTabViewModelTests
{
	[Fact]
	public void ShouldSetSelectedItemFromDrawerInTooling()
	{
		var (drawer, itemChangedSubject) = CreateDrawer();
		var (sideBar, toolingChangedSubject) = CreateSideBar();
		InitializeAnnotationTab(drawer, sideBar);

		var selectedItemConsumer = Substitute.For<SelectedItemConsumer>();
		toolingChangedSubject.OnNext(selectedItemConsumer);

		var item = Substitute.For<BoundedItemDataContext>();
		itemChangedSubject.OnNext(item);

		selectedItemConsumer.Received().SelectedItem = item;
	}

	private static (AnnotationSideBarComponent sideBar, Subject<object> toolingChangedSubject) CreateSideBar()
	{
		var sideBar = Substitute.For<AnnotationSideBarComponent>();
		Subject<object> toolingChangedSubject = new();
		sideBar.AdditionalToolingChanged.Returns(toolingChangedSubject);
		return (sideBar, toolingChangedSubject);
	}

	private static (AnnotationDrawerComponent drawer, Subject<BoundedItemDataContext> itemChangedSubject) CreateDrawer()
	{
		var drawer = Substitute.For<AnnotationDrawerComponent>();
		Subject<BoundedItemDataContext> itemChangedSubject = new();
		drawer.SelectedItemChanged.Returns(itemChangedSubject);
		return (drawer, itemChangedSubject);
	}

	private static void InitializeAnnotationTab(AnnotationDrawerComponent drawer, AnnotationSideBarComponent sideBar)
	{
		_ = new AnnotationTabViewModel(Substitute.For<AnnotationImagesComponent>(), drawer, sideBar);
	}
}