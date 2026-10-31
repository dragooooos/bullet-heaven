using Godot;

[GlobalClass]
public partial class PlayerStats : Resource
{
	[Export]
	public int MaxHealth { get; set; }

	[Export]
	public int Speed { get; set; }
}
