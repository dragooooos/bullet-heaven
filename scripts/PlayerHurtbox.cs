using Godot;
using System.Linq;

public partial class PlayerHurtbox : Hurtbox
{
    [Export]
    public Stats Stats { get; set; }

    [Export]
    public Timer IFrameTimer { get; set; }

    public override void _Ready()
    {
        base._Ready();

        IFrameTimer.WaitTime = Stats.GetStatValue(StatType.InvincibilityDuration);

        IFrameTimer.Timeout += OnIFrameTimerTimeout;
        Stats.StatChanged += OnStatChanged;
    }

    public override void OnAreaEntered(Area2D area)
    {
        if (!IFrameTimer.IsStopped()) return; // Player is currently invincible, ignore damage
        if (HealthComponent.IsDead) return;

        if (area is Hitbox hitbox)
        {
            HealthComponent.TakeDamage(hitbox.Damage);
            StartIFrames();
        }
    }

    private void OnIFrameTimerTimeout()
    {
        if (HealthComponent.IsDead) return;

        SetDeferred("monitorable", true);

        Hitbox[] overlappingHitboxes  = GetOverlappingAreas().OfType<Hitbox>().ToArray();
        if (overlappingHitboxes.Length > 0)
        {
            HealthComponent.TakeDamage(overlappingHitboxes[0].Damage);
            StartIFrames();
        }
    }

    private void OnStatChanged(StatType statType, float newValue)
    {
        if (statType == StatType.InvincibilityDuration)
        {
            IFrameTimer.WaitTime = newValue;
        }
    }

    private void StartIFrames()
    {
        SetDeferred("monitorable", false);
        IFrameTimer.Start();
    }

    public override void _ExitTree()
    {
        IFrameTimer.Timeout -= OnIFrameTimerTimeout;
        Stats.StatChanged -= OnStatChanged;
        AreaEntered -= OnAreaEntered;
    }
}