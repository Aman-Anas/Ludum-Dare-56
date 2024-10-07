using System;
using Game;
using Godot;

public partial class AntEnemy : RigidBody3D, IHealth
{
    [Export]
    public float Health { get; set; }

    [Export]
    Area3D playerDetectArea;

    [Export]
    float speed;

    [Export]
    AnimationTree player;

    BeePlayer bee;

    [Export]
    PackedScene explosion;

    private double timer;

    bool remove = false;

    public override void _Ready()
    {
        playerDetectArea.BodyEntered += Detect;

        BodyEntered += HitSomething;

        // GD.Print("gothit");
    }

    public override void _ExitTree()
    {
        BodyEntered -= HitSomething;
    }

    private void HitSomething(Node body)
    {
        // GD.Print("gothit");
        if (body.HasMeta("Good"))
        {
            Health -= (float)body.GetMeta("Good");

            GD.Print(Health);
        }
        if (Health < 0)
        {
            var boom = explosion.Instantiate<Node3D>();
            GetTree().CurrentScene.AddChild(boom);
            boom.GlobalTransform = GlobalTransform;
            remove = true;
        }
    }

    private void Detect(Node3D body)
    {
        if (body is BeePlayer bee)
        {
            this.bee = bee;
        }
    }

    public override void _Process(double delta)
    {
        // player.
        if (remove)
        {
            QueueFree();
        }

        timer += delta;

        if (bee != null && timer > 0.05)
        {
            // LookAt(bee.GlobalPosition);
            var vecToPlayer = bee.GlobalPosition - GlobalPosition;
            var localVec = GlobalBasis.Inverse() * vecToPlayer;
            localVec.Y = 0;
            // GD.Print(localVec);
            if (localVec.X > 0)
            {
                Rotation = new(0, Rotation.Y - 0.1f, 0);
            }
            else
            {
                Rotation = new(0, Rotation.Y + 0.1f, 0);
            }

            timer = 0;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var localVelo = GlobalBasis.Inverse() * LinearVelocity;
        localVelo.Z = -speed;
        localVelo.X = 0;

        LinearVelocity = GlobalBasis * localVelo;
    }
}
