using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class BoundingBoxViewModel : ViewModel
{
    public BoundingBox BoundingBox { get; }

    public double X1
    {
        get => BoundingBox.X1;
        set
        {
            OnHorizontalPropertiesChanging();
            BoundingBox.SetFromTwoPositions(value, Y1, X2, Y2);
            OnHorizontalPropertiesChanged();
        }
    }

    public double X2
    {
        get => BoundingBox.X2;
        set
        {
            OnHorizontalPropertiesChanging();
            BoundingBox.SetFromTwoPositions(X1, Y1, value, Y2);
            OnHorizontalPropertiesChanged();
        }
    }

    public double Y1
    {
        get => BoundingBox.Y1;
        set
        {
            OnVerticalPropertiesChanging();
            BoundingBox.SetFromTwoPositions(X1, value, X2, Y2);
            OnVerticalPropertiesChanged();
        }
    }

    public double Y2
    {
        get => BoundingBox.Y2;
        set
        {
            OnVerticalPropertiesChanging();
            BoundingBox.SetFromTwoPositions(X1, Y1, X2, value);
            OnVerticalPropertiesChanged();
        }
    }

    public double Width
    {
        get => BoundingBox.Width;
        set
        {
            OnHorizontalPropertiesChanged();
            BoundingBox.SetFromPointAndDimensions(X1, Y1, value, Height);
            OnHorizontalPropertiesChanged();
        }
    }

    public double Height
    {
        get => BoundingBox.Height;
        set
        {
            OnVerticalPropertiesChanging();
            BoundingBox.SetFromPointAndDimensions(X1, Y1, Width, value);
            OnVerticalPropertiesChanged();
        }
    }

    public double XCenter => BoundingBox.XCenter;
    public double YCenter => BoundingBox.YCenter;

    public BoundingBoxViewModel(BoundingBox boundingBox)
    {
        BoundingBox = boundingBox;
    }

    private static readonly string[] HorizontalProperties =
    {
        nameof(X1),
        nameof(X2),
        nameof(Width),
        nameof(XCenter)
    };

    private static readonly string[] VerticalProperties =
    {
        nameof(Y1),
        nameof(Y2),
        nameof(Height),
        nameof(YCenter)
    };

    private void OnHorizontalPropertiesChanging() =>
        OnPropertiesChanging(HorizontalProperties);

    private void OnHorizontalPropertiesChanged() =>
        OnPropertiesChanged(HorizontalProperties);

    private void OnVerticalPropertiesChanging() =>
        OnPropertiesChanging(VerticalProperties);

    private void OnVerticalPropertiesChanged() =>
        OnPropertiesChanged(VerticalProperties);
}