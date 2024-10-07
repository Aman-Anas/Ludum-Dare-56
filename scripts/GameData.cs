namespace Game;

using MemoryPack;

[MemoryPackable]
public partial class GameData
{
    public int LevelOneClearedBits { get; set; } = 0;
    // Add game-related save data here
}
