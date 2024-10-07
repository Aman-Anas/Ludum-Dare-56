using System;
using Godot;

public partial class Win : Node3D
{
    [Export]
    Button returnToTitle;

    [Export(PropertyHint.File)]
    string titleScene;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        returnToTitle.Pressed += () =>
        {
            GetTree().ChangeSceneToFile(titleScene);
        };
    }
}
