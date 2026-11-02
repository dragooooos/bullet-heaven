using Godot;
using System.Collections.Generic;

public partial class Stats : Node
{
    [Signal]
    public delegate void StatChangedEventHandler(StatType statType, float newValue);

    [Export]
    public PlayerStats baseStats { get; set; }

    [Export]
    public Godot.Collections.Array<Upgrade> upgrades { get; set; } = new Godot.Collections.Array<Upgrade>();

    public void AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        NotifyStatChanges(upgrade);
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        if (upgrades.Remove(upgrade))
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

    // public override void _Ready()
    // {
    //     GD.Print(GetStatValue(StatType.Speed));
    // }
}
