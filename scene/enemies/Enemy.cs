using Godot;

namespace RaptorRunPlus.scene.enemies;

public partial class Enemy : CharacterBody2D
{
    [Export] private float _movementSpeed = 1.0f;

    [Export] private int _value = 80;

    [Export] private int _hitTillDie = 1;

    private int _hitsLeft;
    private bool _active = false;
    private bool _isDead = false;
    private AnimatedSprite2D _animatedSprite2D;
    private AudioStreamPlayer2D _deathSound;
    private Player _player;
    private Area2D _hitbox;
    private Game _game;

    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        AddToGroup("enemy");
        _hitsLeft = _hitTillDie;
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetNode<Player>("/root/World/Player");
        _game = GetNode<Game>("/root/World");
        _hitbox = GetNode<Area2D>("%Hitbox");
        _deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
        _hitbox.BodyEntered += OnBodyEntered;
        _player.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        if (_isDead)
        {
            return;
        }

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

    public void Hit()
    {
        if (_hitsLeft <= 1)
        {
            Die();
        }
        else
        {
            _hitsLeft--;
        }
    }

    private void Die()
    {
        if (_isDead)
        {
            return;
        }

        _active = false;
        _isDead = true;
        _game._AddScore(_value);
        _animatedSprite2D.Play("death");
        _animatedSprite2D.AnimationFinished += () => { QueueFree(); };
    }
}