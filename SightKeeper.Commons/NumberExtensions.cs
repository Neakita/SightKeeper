namespace SightKeeper.Commons;

public static class NumberExtensions
{
    public static int Cycle(this int currentValue, int min, int max, bool reverse = false)
    {
        var newValue = reverse ? currentValue - 1 : currentValue + 1;
        return newValue < min ? max : newValue > max ? min : newValue;
    }
}