using System.Reactive.Linq;
using System.Windows.Input;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.Annotation;

public sealed class DeleteSelectedAssetCommandTests
{
	[Fact]
	public void ShouldRaiseCanExecuteChangedWhenSelectedImageAssetsChanged()
	{
		var imageSelection = Substitute.For<ImageSelection>();
		var image = Substitute.For<ManagedImage>();
		imageSelection.SelectedImage.Returns(image);
		imageSelection.SelectedImageChanged.Returns(Observable.Return(image));
		var assets = new FakeAssets();
		image.Assets.Returns(assets);
		var dataSetSelection = Substitute.For<DataSetSelection>();
		var command = new DeleteSelectedAssetCommand(imageSelection, dataSetSelection);
		var monitor = command.Monitor();
		assets.NotifyObserver();
		monitor.Should().Raise(nameof(ICommand.CanExecuteChanged));
	}
}