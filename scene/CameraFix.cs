using System;
using Godot;

namespace RaptorRunPlus.scene;

public partial class CameraFix : Camera2D
{
    private Sprite2D _background;

    private Player _player;

    private Game _game;
    private float _calcQuotient;

    public override void _Ready()
    {
        _background = GetNode<Sprite2D>("../Background");
        _player = GetNode<Player>("../Player");
        _game = GetNode<Game>("/root/World");
        _calcQuotient = (float)((Game.MaxWorldSpeed - Game.MinWorldSpeed) * 0.5);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player.Active)
        {
            Position = new Vector2(_player.Position.x, Math.Min(_player.Position.y, LimitBottom));

            _background.GlobalPosition = new Vector2(
                Position.x,
                Math.Min(_player.Position.y, (LimitBottom - _background.Texture.GetHeight() / 2))
            );
        }

        if (_game.WorldSpeed <= Game.MaxWorldSpeed)
        {
            float offset = (_game.WorldSpeed - Game.MinWorldSpeed) / _calcQuotient;
            float scale = (float)(1.05 + offset);
            float zoom = (float)(1.5 - offset);
            _background.Scale = new Vector2(scale, scale);
            Zoom = new Vector2(zoom, zoom);
        }
    }
}