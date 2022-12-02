using Godot;

namespace RaptorRunPlus.scene;

public partial class Collectible : Area2D
{
    private int _value = 10;

    private Game _game;

    private AnimatedSprite2D _animatedSprite2D;

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _game = GetNode<Game>("/root/World");
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _game._AddScore(_value);
            _animatedSprite2D.Play("collected");
            _animatedSprite2D.AnimationFinished += () => { QueueFree(); };
        }
    }
}