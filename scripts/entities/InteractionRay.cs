using System;
using Game;
using Godot;

public partial class InteractionRay : RayCast3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        // return;
        // Should get the currently held item and call its use/run method. (interface)
        // just testing for now though, so let's assume terraforming

        if (!IsColliding())
        {
            return;
        }

        // GD.Print(collisionObj);
        // CSGBoxes are detected, but aren't actually CollisionObject3Ds
        if (GetCollider() is not CollisionObject3D)
        {
            return;
        }
    }
}
