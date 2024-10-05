using System;
using Game;
using Godot;

public partial class Ring : StaticBody3D
{
    [Export]
    AnimationPlayer player;

    [Export]
    Area3D area;

    bool done = false;

    public override void _Ready()
    {
        area.BodyEntered += potato;
    }

    private void potato(Node3D body)
    {
        if (body is not BeePlayer || done)
        {
            return;
        }
        player.Play("changegreen");
        done = true;
    }
}
