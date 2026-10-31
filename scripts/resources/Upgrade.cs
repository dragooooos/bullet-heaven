using Godot;

[GlobalClass]
public partial class Upgrade : Resource
{
    [Export]
    public string Name { get; set; }

    [Export]
    public Godot.Collections.Array<StatModifier> StatModifiers { get; set; }
}