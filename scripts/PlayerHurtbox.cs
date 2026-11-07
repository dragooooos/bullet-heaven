using Godot;
using System.Linq;

public partial class PlayerHurtbox : Area2D
{
    [Export]
    public HealthComponent HealthComponent { get; set; }

    [Export]
    public Stats Stats { get; set; }

    [Export]
    public Timer IFrameTimer { get; set; }

    public override void _Ready()
    {
        IFrameTimer.WaitTime = Stats.GetStatValue(StatType.InvincibilityDuration);

        IFrameTimer.Timeout += OnIFrameTimerTimeout;
        AreaEntered += OnAreaEntered;
        Stats.StatChanged += OnStatChanged;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (!IFrameTimer.IsStopped()) return; // Player is currently invincible, ignore damage
        if (HealthComponent.IsDead) return;

        if (area is Hitbox hitbox)
        {
            HealthComponent.TakeDamage(hitbox.Damage);
            IFrameTimer.Start();
        }
    }

    private void OnIFrameTimerTimeout()
    {
        if (HealthComponent.IsDead) return;

        Hitbox[] overlappingHitboxes  = GetOverlappingAreas().OfType<Hitbox>().ToArray();
        if (overlappingHitboxes.Length > 0)
        {
            HealthComponent.TakeDamage(overlappingHitboxes[0].Damage);
            IFrameTimer.Start();
        }
    }

    private void OnStatChanged(StatType statType, float newValue)
    {
        if (statType == StatType.InvincibilityDuration)
        {
            IFrameTimer.WaitTime = newValue;
        }
    }

    public override void _ExitTree()
    {
        IFrameTimer.Timeout -= OnIFrameTimerTimeout;
        Stats.StatChanged -= OnStatChanged;
        AreaEntered -= OnAreaEntered;
    }
}