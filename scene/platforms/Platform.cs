using Godot;

namespace RaptorRunPlus.scene.platforms;

public partial class Platform : StaticBody2D
{
    private Game _game;

    public override void _Ready()
    {
        AddToGroup("platform");
        _game = GetNode<Game>("/root/World");
    }

    public override void _Process(double delta)
    {
        if (GlobalPosition.x < -1200)
        {
            _game.SpawnPlatform();
            QueueFree();
        }
    }
}