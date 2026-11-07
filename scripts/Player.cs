using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public HealthComponent HealthComponent { get; private set; }

    [Export]
    public Stats Stats { get; private set; }

    // Get player input
    private void GetInput()
    {
        Vector2 inputVector = Input.GetVector("left", "right", "up", "down");
        Velocity = inputVector * Stats.GetStatValue(StatType.Speed);
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
}
