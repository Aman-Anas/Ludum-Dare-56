using System;
using Game;
using Godot;

public partial class Graphics : GridContainer, SettingsMenu.ISettingsSubMenu
{
    [Export]
    OptionButton windowModeDropdown;

    [Export]
    SpinBox resolutionX;

    [Export]
    SpinBox resolutionY;

    [Export]
    OptionButton vsyncDropdown;

    [Export]
    OptionButton antialiasDropdown;

    [Export]
    SpinBox fpsBox;

    [Export]
    OptionButton locale;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadSettings();

        GetTree().Root.SizeChanged += UpdateResolutionSetting;
    }

    public void LoadSettings()
    {
        var config = Manager.Instance.Config;

        windowModeDropdown.Clear();
        vsyncDropdown.Clear();
        antialiasDropdown.Clear();

        // Populate window mode selecter
        var modeNames = Enum.GetNames<DisplayServer.WindowMode>();
        for (int x = 0; x < modeNames.Length; x++)
        {
            windowModeDropdown.AddItem(modeNames[x], x);
        }
        windowModeDropdown.Selected = (int)config.WindowMode;

        var vsyncNames = Enum.GetNames<DisplayServer.VSyncMode>();
        for (int x = 0; x < vsyncNames.Length; x++)
        {
            vsyncDropdown.AddItem(vsyncNames[x], x);
        }
        vsyncDropdown.Selected = (int)config.VSyncMode;

        var aliasNames = Enum.GetNames<RenderingServer.ViewportMsaa>();
        for (int x = 0; x < aliasNames.Length - 1; x++)
        {
            antialiasDropdown.AddItem(aliasNames[x], x);
        }
        antialiasDropdown.Selected = (int)config.AntiAliasing;

        resolutionX.Value = config.Resolution[0];
        resolutionY.Value = config.Resolution[1];

        fpsBox.Value = config.MaxFPS;
    }

    void UpdateResolutionSetting()
    {
        var config = Manager.Instance.Config;
        var actualRes = DisplayServer.WindowGetSize();
        var mode = DisplayServer.WindowGetMode();

        config.WindowMode = mode;
        config.Resolution = actualRes;

        windowModeDropdown.Selected = (int)mode;
        resolutionX.Value = config.Resolution[0];
        resolutionY.Value = config.Resolution[1];
    }

    public void ApplySettings()
    {
        var config = Manager.Instance.Config;
        config.WindowMode = (DisplayServer.WindowMode)windowModeDropdown.GetSelectedId();
        config.VSyncMode = (DisplayServer.VSyncMode)vsyncDropdown.GetSelectedId();
        config.AntiAliasing = (RenderingServer.ViewportMsaa)antialiasDropdown.GetSelectedId();
        config.Resolution = new((int)resolutionX.Value, (int)resolutionY.Value);
        config.MaxFPS = (int)fpsBox.Value;

        config.UpdateConfig();
    }
}
