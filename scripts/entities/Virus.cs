using System;
using Game;
using Godot;

public partial class Virus : RigidBody3D
{
    [Export]
    Area3D detectPlayerArea;

    [Export]
    float speed;

    [Export]
    PackedScene explosion;

    BeePlayer bee;

    public override void _Ready()
    {
        detectPlayerArea.BodyEntered += Detect;

        BodyEntered += (Node body) =>
        {
            if (body.HasMeta("Good"))
            {
                var boom = explosion.Instantiate<Node3D>();
                GetTree().CurrentScene.AddChild(boom);
                boom.GlobalTransform = GlobalTransform;
                QueueFree();
            }
        };
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
        if (bee == null)
        {
            return;
        }

        state.LinearVelocity = GlobalBasis * new Vector3(0, speed, 0);

        var targetVec = bee.GlobalPosition - GlobalPosition;

        var currentVec = GlobalBasis.Y;
        var diff = new Quaternion(currentVec, targetVec).Normalized();

        GlobalRotate(diff.GetAxis().Normalized(), diff.GetAngle());
    }
}
