namespace Game;

using System;
using Godot;
using Utilities.Collections;

public partial class Manager : Node
{
    public static Manager Instance { get; private set; }

    [Export]
    string configPath = "user://config_user.dat";

    const string defaultConfigPath = "user://config_default.dat";

    public GameConfig Config { get; set; }

    public GameData Data { get; private set; }

    [Export]
    PackedScene titleScene;

    [Export]
    PackedScene mainClientScene;

    [Export]
    PackedScene serverOnlyScene;

    public Manager()
    {
        // Just so that other scripts can cache a reference.
        // Config and game data won't be loaded until _Ready() is called
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // At this point all other autoloads are also ready
        // Now we should do actual game stuff (e.g. loading config)

        // Load config vars
        LoadConfig();

        // Maybe load game data?
        Data = new();
    }

    public void LoadConfig(bool defaultFile = false)
    {
        if (!defaultFile)
        {
            Config = DataUtils.LoadFromFileOrNull<GameConfig>(configPath);
        }

        // Fall back to default config
        if (Config == null || defaultFile)
        {
            Config = DataUtils.LoadFromFileOrNull<GameConfig>(defaultConfigPath);
        }

        // Use current settings if no default either
        Config ??= new();

        Config.UpdateConfig();

        // If there's no default config yet (e.g. first game start)
        if (!FileAccess.FileExists(defaultConfigPath))
        {
            DataUtils.SaveData(defaultConfigPath, Config);
        }
    }

    public void SaveConfig()
    {
        DataUtils.SaveData(configPath, Config);
    }
}
