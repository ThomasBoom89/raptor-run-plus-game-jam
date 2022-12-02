using Godot;

namespace RaptorRunPlus.scene;

public partial class Enemy : CharacterBody2D
{
    private float _movementSpeed = 1.0f;

    private bool _active = false;

    private AnimatedSprite2D _animatedSprite2D;
    private AudioStreamPlayer2D _deathSound;
    private Player _player;
    private Area2D _hitbox;

    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        AddToGroup("enemy");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetNode<Player>("/root/World/Player");
        _hitbox = GetNode<Area2D>("%Hitbox");
        _deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
        _hitbox.BodyEntered += OnBodyEntered;
        _player.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        _animatedSprite2D.Play("idle");
        _active = false;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player") && _active)
        {
            _player.Die();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_active)
        {
            return;
        }

        Velocity = new Vector2(Velocity.x - _movementSpeed, Velocity.y + (Gravity * (float)delta));
        MoveAndSlide();
    }

    public void SetActive()
    {
        _active = true;
        _animatedSprite2D.Play("walk");
    }

    public void Die()
    {
        _active = false;
        _animatedSprite2D.Play("death");
        _animatedSprite2D.AnimationFinished += () => { QueueFree(); };
    }
}