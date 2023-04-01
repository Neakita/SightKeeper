using Avalonia.Input;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.Domain.Model.Common;

[Owned]
public sealed class KeyGesture : ReactiveObject
{
	[Reactive] public Key Key { get; set; }
}