using System;
using Godot;
using Microsoft.VisualBasic;

namespace RaptorRunPlus.scene;

public partial class Game : Node2D
{
    [Signal]
    public delegate void GameOverEventHandler();

    private float _worldSpeed = 300.0f;

    private Node2D _movingEnvironment;

    private PackedScene _platform;
    private PackedScene _platformSingleCollectable;
    private PackedScene _platformRowCollectable;
    private PackedScene _platformRainbowCollectable;
    private PackedScene _platformDinoEnemy;
    private PackedScene _platformPlantEnemy;
    private PackedScene _platformCollectableAmmo;

    private DateTime _lastSpawn;

    private Vector2 _lastPlatformPosition = Vector2.Zero;

    private Random _random;

    private int _score = 0;

    private AudioStreamPlayer2D _collectAudioStreamPlayer2D;
    private Player _player;

    private float _collectiblePitch = 1.0f;

    private DateTime _resetCollectiblePitchDateTime;

    private Label _scoreLabel;
    private Label _gameOverLabel;
    private Label _ammoLabel;

    public override void _Ready()
    {
        _platform = GD.Load<PackedScene>("res://scene/platform.tscn");
        _platformSingleCollectable = GD.Load<PackedScene>("res://scene/platform_collectible_single.tscn");
        _platformRowCollectable = GD.Load<PackedScene>("res://scene/platform_collectible_row.tscn");
        _platformRainbowCollectable = GD.Load<PackedScene>("res://scene/platform_collectible_rainbow.tscn");
        _platformDinoEnemy = GD.Load<PackedScene>("res://scene/platforms/platform_dino_enemy.tscn");
        _platformPlantEnemy = GD.Load<PackedScene>("res://scene/platforms/platform_plant_enemy.tscn");
        _platformCollectableAmmo = GD.Load<PackedScene>("res://scene/platform_collectible_ammo.tscn");
        _movingEnvironment = GetNode<Node2D>("Environment/Moving");
        _player = GetNode<Player>("Player");
        _collectAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Sounds/CollectSound");
        _scoreLabel = GetNode<Label>("HUD/UI/Score");
        _ammoLabel = GetNode<Label>("HUD/UI/Ammo");
        _gameOverLabel = GetNode<Label>("HUD/UI/GameOver");
        _random = new Random();
        _player.PlayerDied += OnPlayerDied;
        SpawnPlatform();
        _lastSpawn = DateTime.Now;
    }

    private void OnPlayerDied()
    {
        EmitSignal(nameof(GameOver));
        _gameOverLabel.Text = $"Game Over. You scored {_score} points! \n\r Press \"Jump\" to restart.";
        _gameOverLabel.Visible = true;
    }

    public override void _Process(double delta)
    {
        if (!_player.Active)
        {
            if (Input.IsActionJustPressed("jump"))
            {
                GetTree().ReloadCurrentScene();
            }

            return;
        }

        if (_resetCollectiblePitchDateTime.Add(new TimeSpan(0, 0, 0, 0, 2000)) < DateTime.Now)
        {
            _collectiblePitch = 1.0f;
        }

        int millis = _random.Next(1000, 2000);
        if (_lastSpawn.Add(new TimeSpan(0, 0, 0, 0, millis)) < DateTime.Now)
        {
            SpawnPlatform();
            _lastSpawn = DateTime.Now;
        }

        _scoreLabel.Text = "Score: " + _score;
        _ammoLabel.Text = "Ammo: " + _player.GetAmmo();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_player.Active)
        {
            return;
        }

        float newX = _movingEnvironment.Position.x - _worldSpeed * (float)delta;
        _movingEnvironment.Position = new Vector2(newX, _movingEnvironment.Position.y);
    }

    private void SpawnPlatform()
    {
        StaticBody2D platform;
        int rand = _random.Next(0, 7);
        switch (rand)
        {
            case 0:
                platform = _platform.Instantiate<StaticBody2D>();
                break;
            case 1:
            default:
                platform = _platformSingleCollectable.Instantiate<StaticBody2D>();
                break;
            case 2:
                platform = _platformRowCollectable.Instantiate<StaticBody2D>();
                break;
            case 3:
                platform = _platformRainbowCollectable.Instantiate<StaticBody2D>();
                break;
            case 4:
                platform = _platformDinoEnemy.Instantiate<StaticBody2D>();
                break;
            case 5:
                platform = _platformCollectableAmmo.Instantiate<StaticBody2D>();
                break;
            case 6:
                platform = _platformPlantEnemy.Instantiate<StaticBody2D>();
                break;
        }

        if (_lastPlatformPosition == Vector2.Zero)
        {
            platform.Position = new Vector2(400, 0);
        }
        else
        {
            float x = _lastPlatformPosition.x + _random.Next(450, 550);
            float y = Math.Clamp(_lastPlatformPosition.y + _random.Next(-150, 150), 200, 1000);
            platform.Position = new Vector2(x, y);
        }

        _movingEnvironment.AddChild(platform);

        _lastPlatformPosition = platform.Position;
    }

    public void _AddScore(int value)
    {
        _score += value;
        _collectAudioStreamPlayer2D.PitchScale = _collectiblePitch;
        _collectAudioStreamPlayer2D.Play();
        _resetCollectiblePitchDateTime = DateTime.Now;
        _collectiblePitch += 0.1f;
    }
}