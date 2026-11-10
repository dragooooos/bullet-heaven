using Godot;

public partial class Hurtbox : Area2D
{
    [Export]
    public HealthComponent HealthComponent { get; set; }

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public virtual void OnAreaEntered(Area2D area)
    {
        if (area is Hitbox hitbox)
        {
            HealthComponent.TakeDamage(hitbox.Damage);
        }
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
    }
}