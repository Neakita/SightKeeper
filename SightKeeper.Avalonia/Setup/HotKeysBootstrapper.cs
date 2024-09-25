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
		// create instances in place instead of registering types to prevent issues when app launches and there is key released which was pressed before app started or instances created
		SimpleReactiveGlobalHook hook = new();
		KeyManagerFilter<FormattedKeyCode> keyboardFilter = new(new SharpHookKeyboardKeyManager(hook));
		KeyManager<FormattedSharpButton> mouseFilter = new KeyManagerFilter<FormattedSharpButton>(new SharpHookMouseButtonsManager(hook));
		AggregateKeyManager aggregateKeyManager = new([keyboardFilter, mouseFilter]);
		GestureManager gestureManager = new(aggregateKeyManager);
		BindingsManager bindingsManager = new(gestureManager);
		hook.RunAsync();
		builder.RegisterInstance(hook).As<IReactiveGlobalHook>();
		builder.RegisterInstance(keyboardFilter).As<KeyManager<FormattedKeyCode>>();
		builder.RegisterInstance(mouseFilter).As<KeyManager<FormattedSharpButton>>();
		builder.RegisterInstance(aggregateKeyManager).As<KeyManager>();
		builder.RegisterInstance(gestureManager);
		builder.RegisterInstance(bindingsManager);
	}
}