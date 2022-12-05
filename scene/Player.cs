using System;
using Godot;

namespace RaptorRunPlus.scene;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void PlayerDiedEventHandler();

    public const float Speed = 300.0f;
    public const float JumpVelocity = -600.0f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D _animatedSprite2D;
    private CollisionShape2D _collisionShape2D;

    private bool _isJumping = false;
    private bool _isDoubleJumping = false;
    private AudioStreamPlayer2D _jumpSound;
    private AudioStreamPlayer2D _deathSound;
    private AudioStreamPlayer2D _shootSound;
    private Node2D _projectilePosition;
    private Camera2D _camera2d;
    private Game _game;
    public bool Active = true;
    private int _ammo = 3;

    private PackedScene _projectile;
    private bool _isFiring = false;

    public override void _Ready()
    {
        AddToGroup("player");
        _projectile = GD.Load<PackedScene>("res://scene/projectile.tscn");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        _jumpSound = GetNode<AudioStreamPlayer2D>("JumpSound");
        _deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
        _shootSound = GetNode<AudioStreamPlayer2D>("ShootSound");
        _projectilePosition = GetNode<Node2D>("ProjectilePosition");
        _camera2d = GetNode<Camera2D>("/root/World/Camera2D");
        _game = GetNode<Game>("/root/World");
        _animatedSprite2D.AnimationFinished += OnAnimationFinished;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (!Active)
        {
            velocity.y += Gravity * (float)delta;
            Velocity = velocity;
            MoveAndSlide();
            return;
        }

        if (!IsOnFloor())
        {
            velocity.y += Gravity * (float)delta;
        }
        else
        {
            if (!_isFiring)
            {
                _animatedSprite2D.Play("run");
            }

            _isJumping = false;
            _isDoubleJumping = false;
        }


        // Handle Double Jump.
        if (_isJumping == true && _isDoubleJumping == false && Input.IsActionJustPressed("jump"))
        {
            _animatedSprite2D.Play("jump");
            _jumpSound.PitchScale = 1.0f;
            _jumpSound.Play();
            _isDoubleJumping = true;
            velocity.y = JumpVelocity;
        }

        // Handle Jump.
        if (_isJumping == false && Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            _animatedSprite2D.Play("double_jump");
            _jumpSound.PitchScale = 1.2f;
            _jumpSound.Play();
            _isJumping = true;
            velocity.y = JumpVelocity;
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("fire") && _ammo > 0)
        {
            _isFiring = true;
            Projectile projectileInstance = _projectile.Instantiate<Projectile>();
            projectileInstance.Position = _projectilePosition.GlobalPosition;
            _ammo--;
            _animatedSprite2D.Play("shoot");
            _shootSound.Play();
            _game.AddChild(projectileInstance);
        }
    }

    private void OnAnimationFinished()
    {
        if (_animatedSprite2D.Animation == "shoot" || _isFiring)
        {
            _isFiring = false;
            _animatedSprite2D.Play("run");
        }
    }

    public void Die()
    {
        _animatedSprite2D.Play("death");
        _deathSound.Play();
        _collisionShape2D.SetDeferred("disabled", true);
        Active = false;
        EmitSignal(nameof(PlayerDied));
    }

    public void AddAmmo()
    {
        _ammo += 3;
    }

    public int GetAmmo()
    {
        return _ammo;
    }
}