namespace Game;

using System;
using Godot;

public partial class Explosion : Node3D
{
    [Export]
    AnimationPlayer player;

    public override void _Ready()
    {
        player.Play("ExplosionAction");

        player.AnimationFinished += RemoveThing;
    }

    public override void _ExitTree()
    {
        player.AnimationFinished -= RemoveThing;
    }

    private void RemoveThing(StringName animName)
    {
        QueueFree();
    }
}
