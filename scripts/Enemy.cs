using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
    public HealthComponent HealthComponent { get; private set; }

    [Export]
    public Stats Stats { get; private set; }

	// Temporary reference to the player which will be passed in from the EnemySpawner in the future.
	[Export]
	public Player Player { get; private set; }

	public override void _PhysicsProcess(double delta)
	{
		Velocity = (Player.GlobalPosition - GlobalPosition).Normalized() * Stats.GetStatValue(StatType.Speed);
		MoveAndSlide();
	}
}
