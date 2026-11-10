using Godot;

[GlobalClass]
public partial class StatModifier : Resource
{
	[Export]
	public StatType StatType { get; set; }

	[Export]
	public ModifierType ModifierType { get; set; }

	[Export]
	public float Value { get; set; }
}
