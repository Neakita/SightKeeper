using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class DrawerViewModelFactoryTests
{
	[Fact]
	public void ShouldNotReturnDrawerWhenNoDataSetProvided()
	{
		var factory = CreateFactory();
		var drawer = factory.CreateDrawerViewModel(null);
		drawer.Should().BeNull();
	}

	[Fact]
	public void ShouldNotReturnDrawerForClassifierDataSet()
	{
		var factory = CreateFactory();
		var drawer = factory.CreateDrawerViewModel(new ClassifierDataSet());
		drawer.Should().BeNull();
	}

	[Fact]
	public void ShouldReturnDetectorDrawer()
	{
		var factory = CreateFactory();
		var drawer = factory.CreateDrawerViewModel(new DetectorDataSet());
		drawer.Should().BeOfType<DetectorDrawerViewModel>();
	}

	[Fact]
	public void ShouldReturnPoser2DDrawer()
	{
		var factory = CreateFactory();
		var drawer = factory.CreateDrawerViewModel(new Poser2DDataSet());
		drawer.Should().BeOfType<Poser2DDrawerViewModel>();
	}

	[Fact]
	public void ShouldReturnPoser3DDrawer()
	{
		var factory = CreateFactory();
		var drawer = factory.CreateDrawerViewModel(new Poser3DDataSet());
		drawer.Should().BeOfType<Poser3DDrawerViewModel>();
	}

	private static DrawerViewModelFactory CreateFactory()
	{
		return new DrawerViewModelFactory(new Composition());
	}
}