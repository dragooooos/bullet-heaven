using Godot;

public partial class HealthComponent : Node
{
    [Signal]
    public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);

    [Signal]
    public delegate void DiedEventHandler();

    [Export]
    public Stats Stats { get; set; } 

    public int CurrentHealth { get; private set; }

    private bool isDead = false;

    public override void _Ready()
    {
        Stats.StatChanged += OnStatChanged;
        CurrentHealth = (int)Stats.GetStatValue(StatType.MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) return; // Ignore negative damage values
        if (isDead) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, (int)Stats.GetStatValue(StatType.MaxHealth));

        EmitSignalHealthChanged(CurrentHealth, (int)Stats.GetStatValue(StatType.MaxHealth));

        if (CurrentHealth == 0)
        {
            isDead = true;
            EmitSignalDied();
        }

    }

    private void OnStatChanged(StatType statType, float newValue)
    {
        if (statType == StatType.MaxHealth)
        {
            int newMaxHealth = (int)newValue;

            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, newMaxHealth);

            // TODO: Heal the player by the difference if the new max health is greater than the current health

            EmitSignalHealthChanged(CurrentHealth, newMaxHealth);
        }
    }

    public override void _ExitTree()
    {
        Stats.StatChanged -= OnStatChanged;
    }
}
