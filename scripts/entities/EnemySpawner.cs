using System;
using Game;
using Godot;

[GlobalClass]
public partial class EnemySpawner : RigidBody3D
{
    [Export]
    public float Health { get; set; }

    [Export]
    PackedScene thingToSpawn;

    [Export]
    float reloadTime;

    [Export]
    Node3D spawnPoint;

    [Export]
    PackedScene explosion;

    public override void _Ready()
    {
        GetTree().CreateTimer(reloadTime, false).Timeout += SpawnThing;

        BodyEntered += GotHit;
    }

    public override void _ExitTree()
    {
        BodyEntered -= GotHit;
    }

    private void GotHit(Node body)
    {
        // GD.Print("oww");
        if (body.HasMeta("Good"))
        {
            Health -= (float)body.GetMeta("Good");
            SpawnThing();
        }
        if (Health < 0)
        {
            var boom = explosion.Instantiate<Node3D>();
            GetTree().CurrentScene.AddChild(boom);
            boom.GlobalTransform = GlobalTransform;
            QueueFree();
        }
    }

    void SpawnThing()
    {
        var thing = thingToSpawn.Instantiate<Node3D>();
        GetTree().CurrentScene.AddChild(thing);
        thing.GlobalTransform = spawnPoint.GlobalTransform;

        GetTree().CreateTimer(reloadTime, false).Timeout += SpawnThing;
    }

    public override void _Process(double delta) { }
}
