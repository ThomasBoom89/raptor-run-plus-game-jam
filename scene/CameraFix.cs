using System;
using Godot;

namespace RaptorRunPlus.scene;

public partial class CameraFix : Camera2D
{
    private Sprite2D _background;

    private Player _player;

    public override void _Ready()
    {
        _background = GetNode<Sprite2D>("../Background");
        _player = GetNode<Player>("../Player");
    }

    public override void _PhysicsProcess(double delta)
    {
        Position = new Vector2(_player.Position.x, Math.Min(_player.Position.y, LimitBottom));

        _background.GlobalPosition = new Vector2(
            Position.x,
            Math.Min(_player.Position.y, (LimitBottom - _background.Texture.GetHeight() / 2))
        );
    }
}