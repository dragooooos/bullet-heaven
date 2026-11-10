using Godot;
using System.Collections.Generic;

public partial class Stats : Node
{
    [Signal]
    public delegate void StatChangedEventHandler(StatType statType, float newValue);

    [Export]
    public BaseStats BaseStats { get; set; }

    [Export]
    public Godot.Collections.Array<Upgrade> Upgrades { get; set; } = new Godot.Collections.Array<Upgrade>();

    public void AddUpgrade(Upgrade upgrade)
    {
        Upgrades.Add(upgrade);
        NotifyStatChanges(upgrade);
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        if (Upgrades.Remove(upgrade))
        {
            NotifyStatChanges(upgrade);
        }
    }

    private void NotifyStatChanges(Upgrade upgrade)
    {
        HashSet<StatType> statTypes = new HashSet<StatType>();

        foreach (StatModifier modifier in upgrade.StatModifiers)
        {
            statTypes.Add(modifier.StatType);
        }

        foreach (StatType statType in statTypes)
        {
            float newValue = GetStatValue(statType);
            EmitSignalStatChanged(statType, newValue);
        }
    }

    // TODO: Implement caching mechanism for stat calculations to improve performance
    public float GetStatValue(StatType statType)
    {
        float baseValue = 0f;
        float flatTotal = 0f;
        float percentageTotal = 0f;
        float multiplicativeTotal = 1f;

        switch (statType)
        {
            case StatType.MaxHealth:
                baseValue = BaseStats.MaxHealth;
                break;
            case StatType.Speed:
                baseValue = BaseStats.Speed;
                break;
            case StatType.InvincibilityDuration:
                baseValue = BaseStats.InvincibilityDuration;
                break;
        }

        foreach (Upgrade upgrade in Upgrades)
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
