using Godot;

namespace RaptorRunPlus.scene;

public partial class AmmoCollectible : Area2D
{
    private AnimatedSprite2D _animatedSprite2D;

    private AudioStreamPlayer2D _pickupSound;

    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/World/Player");
        _pickupSound = GetNode<AudioStreamPlayer2D>("PickupSound");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _player.AddAmmo();
            _pickupSound.Play();
            _animatedSprite2D.Play("collected");
            _animatedSprite2D.AnimationFinished += () => { QueueFree(); };
        }
    }
}