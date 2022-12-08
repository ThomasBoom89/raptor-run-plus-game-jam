using System;
using Godot;

namespace RaptorRunPlus.scene;

public partial class CameraFix : Camera2D
{
    private const float MinZoom = 1.0f;

    private Sprite2D _background;

    private Player _player;

    private Game _game;
    private float _calcQuotient;

    public override void _Ready()
    {
        _background = GetNode<Sprite2D>("../BackgroundCanvasLayer/Background");
        _player = GetNode<Player>("../Player");
        _game = GetNode<Game>("/root/World");
        _calcQuotient = (float)((Game.MaxWorldSpeed - Game.MinWorldSpeed) * 0.8);
        Current = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player.Active)
        {
            Position = new Vector2(_player.Position.x, Math.Min(_player.Position.y, LimitBottom));
        }

        if (_game.WorldSpeed <= Game.MaxWorldSpeed && Zoom.x > MinZoom)
        {
            float offset = (_game.WorldSpeed - Game.MinWorldSpeed) / _calcQuotient;
            float zoom = (float)(1.5 - offset);
            Zoom = new Vector2(zoom, zoom);
        }
    }
}