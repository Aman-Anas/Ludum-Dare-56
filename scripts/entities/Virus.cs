using System;
using Game;
using Godot;

public partial class Virus : RigidBody3D
{
    [Export]
    Area3D detectPlayerArea;

    [Export]
    float speed;

    BeePlayer bee;

    public override void _Ready()
    {
        detectPlayerArea.BodyEntered += Detect;
    }

    public override void _ExitTree()
    {
        detectPlayerArea.BodyEntered -= Detect;
    }

    private void Detect(Node3D body)
    {
        if (body is BeePlayer bee)
        {
            this.bee = bee;
        }
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        state.ApplyCentralForce(-state.TotalGravity);
        state.LinearVelocity = GlobalBasis * new Vector3(0, speed, 0);
    }
}
