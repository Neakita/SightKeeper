namespace SightKeeper.Domain.Model.Profiles.Actions;

public sealed class PressKeyAction : Action
{
	public byte Type { get; set; }
	public ushort KeyCode { get; set; }

	public PressKeyAction(byte type, ushort keyCode)
	{
		Type = type;
		KeyCode = keyCode;
	}
}