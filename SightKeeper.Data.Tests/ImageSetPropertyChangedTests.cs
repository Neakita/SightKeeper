using System.ComponentModel;
using FluentAssertions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

public sealed class ImageSetPropertyChangedTests
{
	[Fact]
	public void ShouldObserveNameChange()
	{
		var set = Utilities.CreateImageSet();

		List<PropertyChangedEventArgs> args = new();

		((INotifyPropertyChanged)set).PropertyChanged += OnPropertyChanged;

		set.Name = "New name";

		((INotifyPropertyChanged)set).PropertyChanged -= OnPropertyChanged;

		args.Should().Contain(e => e.PropertyName == nameof(ImageSet.Name));

		void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			args.Add(e);
		}
	}

	[Fact]
	public void ShouldObserveDescriptionChange()
	{
		var set = Utilities.CreateImageSet();

		List<PropertyChangedEventArgs> args = new();

		((INotifyPropertyChanged)set).PropertyChanged += OnPropertyChanged;

		set.Description = "New description";

		((INotifyPropertyChanged)set).PropertyChanged -= OnPropertyChanged;

		args.Should().Contain(e => e.PropertyName == nameof(ImageSet.Description));

		void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			args.Add(e);
		}
	}
}