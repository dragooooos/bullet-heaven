using Godot;

public partial class Stats : Node
{
    [Export]
    public PlayerStats baseStats { get; set; }

    public Godot.Collections.Array<Upgrade> upgrades { get; set; } = new Godot.Collections.Array<Upgrade>();

    public void AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        upgrades.Remove(upgrade);
    }

    public float GetStatValue(StatType statType)
    {
        float baseValue = 0f;
        float flatTotal = 0f;
        float percentageTotal = 0f;
        float multiplicativeTotal = 1f;

        switch (statType)
        {
            case StatType.Health:
                baseValue = baseStats.MaxHealth;
                break;
            case StatType.Speed:
                baseValue = baseStats.Speed;
                break;
        }

        foreach (Upgrade upgrade in upgrades)
        {
            foreach (StatModifier modifier in upgrade.StatModifiers)
            {
                if (modifier.StatType == statType)
                {
                    switch (modifier.ModifierType)
                    {
                        case ModifierType.Additive:
                            flatTotal += modifier.Value;
                            break;
                        case ModifierType.Percentage:
                            percentageTotal += modifier.Value;
                            break;
                        case ModifierType.Multiplicative:
                            multiplicativeTotal *= modifier.Value;
                            break;
                    }
                }
            }
        }

        return (baseValue + flatTotal) * (1f + percentageTotal / 100f) * multiplicativeTotal;
    }
}
