using Godot;
using System;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);

	[Signal]
	public delegate void DiedEventHandler();

	[Export]
	public Stats Stats { get; set; } 

	public int CurrentHealth { get; private set; }

	public bool IsDead = false;

	private float previousMaxHealth;

	public override void _Ready()
	{
		Stats.StatChanged += OnStatChanged;
		CurrentHealth = (int)Stats.GetStatValue(StatType.MaxHealth);
		previousMaxHealth = (int)Stats.GetStatValue(StatType.MaxHealth);
	}

	public void TakeDamage(int damage)
	{
		if (damage < 0) return; // Ignore negative damage values
		if (IsDead) return;

		CurrentHealth -= damage;
		CurrentHealth = Mathf.Clamp(CurrentHealth, 0, (int)Stats.GetStatValue(StatType.MaxHealth));

		GD.Print($"Player took {damage} damage. Current health: {CurrentHealth}/{(int)Stats.GetStatValue(StatType.MaxHealth)}");
		EmitSignalHealthChanged(CurrentHealth, (int)Stats.GetStatValue(StatType.MaxHealth));

		if (CurrentHealth == 0)
		{
			IsDead = true;
			EmitSignalDied();
		}

	}

	private void OnStatChanged(StatType statType, float newValue)
	{
		if (IsDead) return;

		if (statType == StatType.MaxHealth)
		{
			int newMaxHealth = (int)newValue;

			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, newMaxHealth);

			// Heal the player by the difference between the new max health and the previous max health
			if (previousMaxHealth < newMaxHealth)
			{
				CurrentHealth += (int)(newMaxHealth - previousMaxHealth);
				CurrentHealth = Mathf.Clamp(CurrentHealth, 0, newMaxHealth);
			}
			previousMaxHealth = newMaxHealth;

			EmitSignalHealthChanged(CurrentHealth, newMaxHealth);
		}
	}

	public override void _ExitTree()
	{
		Stats.StatChanged -= OnStatChanged;
	}
}
