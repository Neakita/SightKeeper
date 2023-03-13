using Avalonia.Input;
using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Domain.Model.Common;

[Owned]
public sealed class KeyGesture
{
	public Key Key { get; set; }
}