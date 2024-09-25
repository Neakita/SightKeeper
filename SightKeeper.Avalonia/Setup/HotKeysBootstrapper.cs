using Autofac;
using HotKeys;
using HotKeys.Bindings;
using HotKeys.Gestures;
using HotKeys.SharpHook;
using SharpHook.Reactive;

namespace SightKeeper.Avalonia.Setup;

internal static class HotKeysBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		SimpleReactiveGlobalHook hook = new();
		hook.RunAsync();
		builder.RegisterInstance(hook).As<IReactiveGlobalHook>();
		SharpHookKeyboardKeyManager keyboardManager = new(hook);
		KeyManagerFilter<FormattedKeyCode> keyboardFilter = new(keyboardManager);
		builder.RegisterInstance(keyboardFilter).As<KeyManager<FormattedKeyCode>>();
		SharpHookMouseButtonsManager mouseButtonsManager = new(hook);
		KeyManager<FormattedSharpButton> mouseFilter = new KeyManagerFilter<FormattedSharpButton>(mouseButtonsManager);
		builder.RegisterInstance(mouseFilter).As<KeyManager<FormattedSharpButton>>();
		AggregateKeyManager aggregateKeyManager = new([keyboardFilter, mouseFilter]);
		builder.RegisterInstance(aggregateKeyManager).As<KeyManager>();
		builder.RegisterType<GestureManager>().SingleInstance();
		builder.RegisterType<BindingsManager>().SingleInstance();
	}
}