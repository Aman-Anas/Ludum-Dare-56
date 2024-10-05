using System;
using Game;
using Godot;
using Utilities;

public partial class CameraGameSmooth : Node3D
{
    [Export]
    Node3D target;

    [Export]
    BeePlayer player;

    public override void _PhysicsProcess(double delta)
    {
        var targetPos = target.GlobalPosition;
        var currentPos = GlobalPosition;
        GlobalPosition = targetPos;

        var targetRot = target.GlobalBasis.GetRotationQuaternion(); //new Quaternion(target.GlobalBasis);
        var currentRot = GlobalBasis.GetRotationQuaternion();
        GlobalBasis = new Basis(currentRot.Slerp(targetRot, 0.06f + (0.76f * player.SpeedFloat)));
    }
}
