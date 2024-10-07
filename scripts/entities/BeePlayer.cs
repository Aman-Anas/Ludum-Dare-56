namespace Game;

using System;
using System.Linq;
using Game;
using Godot;

[GlobalClass]
public partial class BeePlayer : RigidBody3D
{
    public float Health { get; set; } = 100;

    public Action GotHurt { get; set; }

    public bool MovementEnabled { get; set; } = true;

    public float SpeedFloat { get; internal set; }

    [Export]
    PewShooter pewPos1;

    [Export]
    PewShooter pewPos2;

    bool readyFire = true;

    // Mouselook
    Vector2 mouseMovement = new();

    public const float MOVEMENT_SPEED = 60f;
    const float MOVEMENT_FORCE = 40f;
    const float ROTATION_SPEED = 3f;

    bool restart = false;

    // Player Animation
    [Export]
    AnimationPlayer animPlayer;

    [Export]
    PackedScene explosion;

    readonly StringName DEFAULT_ANIM = new("Idle"); // This should be an idle
    readonly StringName RUNNING_ANIM = new("Run");

    [Export]
    AudioStreamPlayer pewSound;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Need this to capture the mouse of course
        Input.MouseMode = Input.MouseModeEnum.Captured;
        this.BodyEntered += HitSomething;
    }

    public override void _ExitTree()
    {
        BodyEntered -= HitSomething;
    }

    public void HitSomething(Node body)
    {
        if (body.HasMeta("Bad"))
        {
            Health -= (float)body.GetMeta("Bad");
            GotHurt?.Invoke();

            if (Health < 0)
            {
                var boom = explosion.Instantiate<Node3D>();
                GetTree().CurrentScene.AddChild(boom);
                boom.GlobalTransform = GlobalTransform;
                restart = true;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
    {
        if (restart)
        {
            // foreach (Node theNode in GetTree().CurrentScene.GetChildren())
            // {
            //     GD.Print(theNode);
            //     theNode.Free();
            // }
            GetTree().ReloadCurrentScene();
            return;
        }

        RunAnimations();

        if (Input.IsActionPressed(GameActions.PLAYER_PRIMARY_USE) && readyFire)
        {
            pewPos1.Shoot();
            pewPos2.Shoot();

            pewSound.Play();

            readyFire = false;
            GetTree().CreateTimer(0.1, false).Timeout += () => readyFire = true;
        }
        SpeedFloat = Mathf.Abs(-(GlobalBasis.Inverse() * LinearVelocity).Z) / MOVEMENT_SPEED;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!MovementEnabled)
            return;

        // Direct mouselook for head itself
        if (@event is InputEventMouseMotion motion)
        {
            var sensitivity = Manager.Instance.Config.MouseSensitivity;

            // GD.Print(motion.ScreenRelative);
            if (motion.ScreenRelative.Length() < 2)
            {
                mouseMovement = Vector2.Zero;
            }
            else
            {
                mouseMovement = sensitivity * motion.ScreenRelative;
            }
        }
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        if (!MovementEnabled)
            return;

        Orthonormalize();

        var forward = Input.GetAxis(GameActions.PLAYER_FORWARD, GameActions.PLAYER_BACKWARD);
        // var strafe = Input.GetAxis(GameActions.PLAYER_STRAFE_LEFT, GameActions.PLAYER_STRAFE_RIGHT);
        var roll = -Input.GetAxis(GameActions.PLAYER_STRAFE_LEFT, GameActions.PLAYER_STRAFE_RIGHT);

        var yaw = -mouseMovement.X * ROTATION_SPEED;
        var pitch = -mouseMovement.Y * ROTATION_SPEED;

        var gravityForce = state.TotalGravity;

        var newAngularVel = GlobalBasis * (ROTATION_SPEED * new Vector3(pitch, yaw, roll));

        var diffAngular = newAngularVel - state.AngularVelocity;

        // Set our angular velocity
        state.AngularVelocity += diffAngular * 0.4f;

        var targetLinearVel = MOVEMENT_SPEED * new Vector3(0, 0, forward);
        var actualLinearVel = GlobalBasis.Inverse() * state.LinearVelocity;

        var diffVelo = targetLinearVel - actualLinearVel;

        state.ApplyCentralForce(GlobalBasis * (diffVelo.LimitLength(1) * MOVEMENT_FORCE));
        state.ApplyCentralForce(-gravityForce);

        var currentLin = GlobalBasis.Inverse() * state.LinearVelocity;
        currentLin.X = 0;
        currentLin.Y = 0;
        state.LinearVelocity = GlobalBasis * currentLin;

        mouseMovement = Vector2.Zero;
    }

    void RunAnimations()
    {
        // var horizontalSpeed = GlobalBasis.Inverse() * LinearVelocity;
        // horizontalSpeed.Y = 0;
        // if (movemen.Length() > 0.1 && (horizontalSpeed.Length() > 0.2))
        // {
        //     animPlayer.Play(RUNNING_ANIM);
        // }
        // else
        // {
        //     animPlayer.Play(DEFAULT_ANIM);
        // }
    }
}
