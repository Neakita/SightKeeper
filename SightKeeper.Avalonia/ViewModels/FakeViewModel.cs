using System.Windows.Input;
using NSubstitute;

namespace SightKeeper.Avalonia.ViewModels;

public static class FakeViewModel
{
    internal static ICommand CommandSubstitute { get; } = Substitute.For<ICommand>();
}