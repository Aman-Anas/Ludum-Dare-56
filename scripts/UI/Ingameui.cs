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

    [Export]
    TextureProgressBar hpBar;

    [Export]
    AnimationPlayer anims;

    public override void _Ready()
    {
        player.GotHurt += () => anims.Play("hurt");
    }

    public override void _Process(double delta)
    {
        int speed = Mathf.RoundToInt(101 * player.SpeedFloat);

        speedBar.Value = speed;

        speedNum.Text = speed.ToString();

        hpBar.Value = player.Health;
    }
}
