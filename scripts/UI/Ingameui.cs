using System;
using Game;
using Godot;

public partial class Ingameui : Control
{
    [Export]
    BeePlayer player;

    [Export]
    TextureProgressBar speedBar;

    [Export]
    Label speedNum;

    public override void _Process(double delta)
    {
        int speed = Mathf.RoundToInt(101 * player.SpeedFloat);

        speedBar.Value = speed;

        speedNum.Text = speed.ToString();
    }
}
