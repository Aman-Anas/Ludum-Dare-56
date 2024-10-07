using System;
using Game;
using Godot;

public partial class EvilPew : RigidBody3D
{
    public float Speed { get; set; } = 0;

    public override void _Ready()
    {
        BodyEntered += (Node body) =>
        {
            // if (body is BeePlayer bee)
            // {
            //     bee.HitSomething(this);
            // }
            QueueFree();
        };
    }

    public override void _Process(double delta)
    {
        Position += Basis.Z * Speed * (float)delta;
    }
}
