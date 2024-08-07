using Avalonia;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating.Drawer;

internal sealed class BoundingViewModel : ViewModel
{
    private static readonly string[] Properties =
    {
        nameof(Left),
        nameof(Right),
        nameof(Top),
        nameof(Bottom),
        nameof(Width),
        nameof(Height),
        nameof(XCenter),
        nameof(YCenter)
    };

    public DetectorItem? Item { get; set; }
    public Bounding Bounding { get; set; }

    public double Left
    {
        get => _left;
        set
        {
            Guard.IsLessThanOrEqualTo(value, Right);
            if (!SetProperty(ref _left, value))
                return;
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(XCenter));
        }
    }

    public double Top
    {
        get => _top;
        set
        {
            Guard.IsLessThanOrEqualTo(value, Bottom);
            if (!SetProperty(ref _top, value))
                return;
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(YCenter));
        }
    }

    public double Right
    {
        get => _right;
        set
        {
            Guard.IsGreaterThanOrEqualTo(value, Left);
            if (!SetProperty(ref _right, value))
                return;
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(XCenter));
        }
    }

    public double Bottom
    {
        get => _bottom;
        set
        {
            Guard.IsGreaterThanOrEqualTo(value, Top);
            if (!SetProperty(ref _bottom, value))
                return;
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(YCenter));
        }
    }

    public double Width => Right - Left;
    public double Height => Bottom - Top;
    public double XCenter => (Left + Right) / 2;
    public double YCenter => (Top + Bottom) / 2;

    public BoundingViewModel(Point position) : this()
    {
        _left = position.X;
        _right = position.X;
        _top = position.Y;
        _bottom = position.Y;
    }

    public BoundingViewModel(Bounding bounding)
    {
        Bounding = bounding;
        _left = bounding.Left;
        _top = bounding.Top;
        _right = bounding.Right;
        _bottom = bounding.Bottom;
    }

    public BoundingViewModel()
    {
        Bounding = new Bounding();
    }

    public void SetFromTwoPositions(Point position1, Point position2) =>
        SetFromTwoPositions(position1.X, position1.Y, position2.X, position2.Y);

    public void SetFromTwoPositions(double x1, double y1, double x2, double y2)
    {
        MinMax(x1, x2, out var xMin, out var xMax);
        MinMax(y1, y2, out var yMin, out var yMax);
        OnPropertiesChanging(Properties);
        _left = xMin;
        _right = xMax;
        _top = yMin;
        _bottom = yMax;
        OnPropertiesChanged(Properties);
    }

    public void Synchronize()
    {
        Guard.IsNotNull(Item);
        Item.Bounding = Bounding;
    }

    private double _left;
    private double _top;
    private double _right;
    private double _bottom;

    private static void MinMax(double value1, double value2, out double min, out double max)
    {
        if (value1 < value2)
        {
            min = value1;
            max = value2;
        }
        else
        {
            min = value2;
            max = value1;
        }
    }
}