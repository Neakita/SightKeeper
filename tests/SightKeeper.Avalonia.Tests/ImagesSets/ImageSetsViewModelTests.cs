using System.Collections.Specialized;
using System.Windows.Input;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application;
using SightKeeper.Avalonia.ImageSets;
using SightKeeper.Avalonia.ImageSets.Capturing;
using SightKeeper.Avalonia.ImageSets.Card;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.ImagesSets;

public sealed class ImageSetsViewModelTests
{
	[Fact]
	public void ImageSetsShouldBeNotifyCollectionChanged()
	{
		ImageSetsViewModel viewModel = new(
			Substitute.For<ICommand>(),
			Substitute.For<ObservableListRepository<ImageSet>>(),
			Substitute.For<ImageSetCardDataContextFactory>(),
			Substitute.For<CapturingSettingsDataContext>());
		viewModel.ImageSets.Should().BeAssignableTo<INotifyCollectionChanged>();
	}
}