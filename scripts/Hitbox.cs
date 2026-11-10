using Godot;
using System;

public partial class Hitbox : Area2D
{   
	[Signal]
	public delegate void HitLandedEventHandler(int damage);

	[Export]
	public int Damage { get; set; }

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Hurtbox)
		{
			EmitSignalHitLanded(Damage);
		}
	}
}
