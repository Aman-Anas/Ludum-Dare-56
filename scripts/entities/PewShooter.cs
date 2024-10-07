using System;
using Game;
using Godot;

[GlobalClass]
public partial class PewShooter : Node3D
{
    [Export]
    PackedScene pew;

    public void Shoot()
    {
        var newPew = pew.Instantiate<RigidBody3D>();
        GetTree().CurrentScene.AddChild(newPew);

        newPew.GlobalTransform = GlobalTransform;

        GetTree().CreateTimer(3, false).Timeout += newPew.QueueFree;
        newPew.LinearVelocity = newPew.GlobalBasis * new Vector3(0, 0, -60f);
    }
}
