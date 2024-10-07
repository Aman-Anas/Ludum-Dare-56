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

    [Export]
    int maxToSpawn;

    [Export]
    AnimationPlayer anims;

    public override void _Ready()
    {
        GetTree().CreateTimer(reloadTime, false).Timeout += SpawnThingRepeat;

        // BodyEntered += GotHit;
    }

    public override void _ExitTree()
    {
        // BodyEntered -= GotHit;
    }

    StringName good = new("Good");
    StringName hurt = new("hurt");

    private void GotHit(Node body)
    {
        // GD.Print("oww");
        if (body.HasMeta(good))
        {
            Health -= (float)body.GetMeta(good);
            anims.Play(hurt);
        }
        if (Health < 0)
        {
            var boom = explosion.Instantiate<Node3D>();
            GetTree().CurrentScene.AddChild(boom);
            boom.GlobalTransform = GlobalTransform;
            for (int x = 0; x < 5; x++)
            {
                SpawnThing();
            }
            Manager.Instance.Data.LevelOneClearedBits++;
            Manager.Instance.UpdateCount?.Invoke();
            Free();
        }
    }

    void SpawnThing()
    {
        var thing = thingToSpawn.Instantiate<Node3D>();
        GetTree().CurrentScene.AddChild(thing);
        thing.GlobalTransform = spawnPoint.GlobalTransform;
    }

    void SpawnThingRepeat()
    {
        SpawnThing();
        maxToSpawn--;
        if (maxToSpawn > 0)
        {
            GetTree().CreateTimer(reloadTime, false).Timeout += SpawnThingRepeat;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var colliding = GetCollidingBodies();
        if (colliding.Count > 0)
        {
            GotHit(colliding[0]);
        }
    }
}
