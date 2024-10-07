using System;
using Game;
using Godot;

public partial class EnemyShooter : MeshInstance3D
{
    [Export]
    PackedScene thingToLaunch;

    [Export]
    Area3D detectPlayerArea;

    [Export]
    float reloadTime;

    [Export]
    float velocity;

    [Export]
    Node3D spawnPoint;

    [Export]
    float timeDespawn;
    BeePlayer bee;

    public override void _Ready()
    {
        GetTree().CreateTimer(reloadTime, false).Timeout += SpawnThing;
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

            // LookAt(bee.GlobalPosition, useModelFront: true);
        }
    }

    void SpawnThing()
    {
        var thing = thingToLaunch.Instantiate<RigidBody3D>();
        GetTree().CurrentScene.AddChild(thing);
        thing.GlobalTransform = spawnPoint.GlobalTransform;

        // Add speed
        var localVelo = new Vector3(0, 0, velocity);
        thing.LinearVelocity = thing.GlobalBasis * localVelo;
        thing.GetTree().CreateTimer(timeDespawn, false).Timeout += thing.QueueFree;

        thing.GetTree().CreateTimer(reloadTime, false).Timeout += SpawnThing;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (bee == null)
        {
            return;
        }

        var targetVec = bee.GlobalPosition - GlobalPosition;
        var currentVec = GlobalBasis.Y;
        var diff = new Quaternion(currentVec, targetVec).Normalized();
        GlobalBasis = new Basis(GlobalBasis.GetRotationQuaternion() + diff);
    }
}
