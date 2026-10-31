using Godot;

public partial class Player : CharacterBody2D
{
    private HealthComponent _healthComponent;
    private Stats _stats;

    // Get player input
    private void GetInput()
    {
        Vector2 inputVector = Input.GetVector("left", "right", "up", "down");
        Velocity = inputVector * _stats.GetStatValue(StatType.Speed);
    }

    public override void _Ready()
    {
        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        _stats = GetNode<Stats>("Stats");
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
}
