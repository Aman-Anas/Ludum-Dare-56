using System;
using Game;
using Godot;

public partial class EvilDonut : RigidBody3D
{
    [Export]
    float health = 500;

    [Export]
    PackedScene explosion;

    [Export]
    TextureProgressBar bossHpBar;

    [Export]
    BeePlayer bee;

    [Export]
    PackedScene thingToLaunch;

    [Export]
    PackedScene virus;

    public override void _Ready()
    {
        BodyEntered += (Node body) =>
        {
            if (body.HasMeta("Good"))
            {
                health -= (float)body.GetMeta("Good");
                bossHpBar.Value = health;
            }
        };
        GetTree().CreateTimer(0, false).Timeout += SpawnThing;

        GetTree().CreateTimer(0, false).Timeout += VirusSpawn;
    }

    private void VirusSpawn()
    {
        var thing = virus.Instantiate<Virus>();
        GetTree().CurrentScene.AddChild(thing);
        thing.GlobalTransform = GlobalTransform;

        GetTree().CreateTimer(8, false).Timeout += VirusSpawn;
    }

    void SpawnThing()
    {
        var thing = thingToLaunch.Instantiate<EvilPew>();
        GetTree().CurrentScene.AddChild(thing);
        thing.GlobalTransform = GlobalTransform;
        thing.ScaleObjectLocal(Vector3.One * 10f);

        // Add speed
        // var localVelo = new Vector3(0, 0, velocity);
        thing.Speed = 3f;
        thing.GetTree().CreateTimer(3, false).Timeout += thing.QueueFree;

        GetTree().CreateTimer(0.6, false).Timeout += SpawnThing;
    }

    public override void _Process(double delta)
    {
        if (health > 0)
        {
            return;
        }
        var boom = explosion.Instantiate<Node3D>();
        boom.ScaleObjectLocal(Vector3.One * 10f);
        GetTree().CurrentScene.AddChild(boom);
        boom.GlobalTransform = GlobalTransform;
        QueueFree();
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        state.ApplyCentralForce(-state.TotalGravity);

        var targetVec = bee.GlobalPosition - GlobalPosition;

        var currentVec = GlobalBasis.Z;
        var diff = new Quaternion(currentVec, targetVec).Normalized();

        GlobalRotate(diff.GetAxis().Normalized(), diff.GetAngle());
    }
}
