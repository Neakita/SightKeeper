using System;
using System.Linq;
using CommunityToolkit.Diagnostics;
using HotKeys;
using SharpHook.Data;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

internal sealed class FormattedSharpHookGesture(Gesture gesture)
{
	public Gesture Gesture { get; } = gesture;

	public override string ToString()
	{
		return string.Join(" + ", Gesture.Keys.Select(FormatKey));
	}

	private static string? FormatKey(object key)
	{
		return key switch
		{
			KeyCode keyCode => FormatKeyCode(keyCode),
			MouseButton mouseButton => FormatMouseButton(mouseButton),
			_ => key.ToString()
		};
	}

	private static string FormatKeyCode(KeyCode keyCode)
	{
		var str = keyCode.ToString();
		Guard.IsTrue(str.StartsWith("Vc"));
		return str[2..];
	}

	private static string FormatMouseButton(MouseButton mouseButton)
	{
		return mouseButton switch
		{
			MouseButton.Button1 => "LButton",
			MouseButton.Button2 => "RButton",
			MouseButton.Button3 => "MButton",
			MouseButton.Button4 => "XButton1",
			MouseButton.Button5 => "XButton2",
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}