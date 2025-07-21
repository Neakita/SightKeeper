using System;
using System.ComponentModel;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation;

public sealed class ImageSetViewModel : ViewModel, ImageSetDataContext, IDisposable
{
	public ImageSet Value { get; }
	public string Name => Value.Name;

	public ImageSetViewModel(ImageSet value)
	{
		Value = value;
		if (value is INotifyPropertyChanged notifyingValue)
			notifyingValue.PropertyChanged += OnSetPropertyChanged;
	}

	public void Dispose()
	{
		if (Value is INotifyPropertyChanged notifyingValue)
			notifyingValue.PropertyChanged -= OnSetPropertyChanged;
	}

	private void OnSetPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ImageSet.Name))
			OnPropertyChanged(args);
	}
}