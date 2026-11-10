using Godot;

[GlobalClass]
public partial class BaseStats : Resource
{
	[Export]
	public int MaxHealth { get; set; }

	[Export]
	public float Speed { get; set; }

	[Export]
	public float InvincibilityDuration { get; set; }
}
