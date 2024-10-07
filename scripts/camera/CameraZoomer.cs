using System;
using Godot;

public partial class CameraZoomer : Camera3D
{
    [Export]
    RayCast3D cast;

    public override void _Ready()
    {
        GetTree().CreateTimer(0.1, false).Timeout += () =>
        {
            GlobalPosition = ((Node3D)GetParent()).GlobalPosition;
        };
    }

    public override void _Process(double delta)
    {
        if (cast.IsColliding())
        {
            var pos = GlobalBasis.Inverse() * GlobalPosition;
            var point = cast.GetCollisionPoint();
            var localPoint = GlobalBasis.Inverse() * point;

            pos.Z = localPoint.Z - 0.2f;
            GlobalPosition = GlobalBasis * pos;
        }
        else
        {
            var pos = Position;
            if (pos.Z < 0.3)
            {
                pos.Z += (float)(1f * delta);
            }
            else if (pos.Z > -0.3)
            {
                pos.Z -= (float)(1f * delta);
            }
            Position = pos;
        }
    }
}
