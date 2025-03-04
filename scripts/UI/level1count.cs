using System;
using Game;
using Godot;

public partial class level1count : Label
{
    public override void _Ready()
    {
        Manager.Instance.UpdateCount += UpdateThing;
    }

    void UpdateThing()
    {
        var amt = Manager.Instance.Data.LevelOneClearedBits;
        if (amt == 17)
        {
            Text = $"ESCAPE!!";
        }
        else
        {
            Text = $"{Manager.Instance.Data.LevelOneClearedBits} of 17 anthills destroyed!";
        }
    }

    public override void _ExitTree()
    {
        Manager.Instance.UpdateCount -= UpdateThing;
    }
}
