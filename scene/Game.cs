using System;
using Godot;
using RaptorRunPlus.scene.service;

namespace RaptorRunPlus.scene;

public partial class Game : Node2D
{
    [Signal]
    public delegate void GameOverEventHandler();

    public float WorldSpeed;
    private float _worldSpeedStep = 1.0f;
    public const float MinWorldSpeed = 300.0f;
    public const float MaxWorldSpeed = 900.0f;

    private Node2D _movingEnvironment;

    private PackedScene _platform;
    private PackedScene _platformSingleCollectable;
    private PackedScene _platformRowCollectable;
    private PackedScene _platformRainbowCollectable;
    private PackedScene _platformDinoEnemy;
    private PackedScene _platformPlantEnemy;
    private PackedScene _platformCollectableAmmo;

    private PackedScene _pauseMenu;
    private Node2D _pauseMenuInstance;

    private Vector2 _lastPlatformPosition = Vector2.Zero;

    private Random _random;

    private int _score = 0;

    private AudioStreamPlayer2D _collectAudioStreamPlayer2D;
    private Player _player;
    private Camera2D _camera2D;

    private float _collectiblePitch = 1.0f;

    private ulong _resetCollectiblePitchTime;

    private Label _scoreLabel;
    private Label _gameOverLabel;
    private Label _ammoLabel;

    private ulong _startTime;
    private ulong _timeTillSpeedUpdate = 100;

    private GameState _gameState;

    public Game()
    {
        _gameState = DIContainer.GetFactory().GetGameState();
    }

    public override void _Ready()
    {
        WorldSpeed = MinWorldSpeed;
        _startTime = Time.GetTicksMsec();
        _platform = GD.Load<PackedScene>("res://scene/platforms/platform.tscn");
        _platformSingleCollectable = GD.Load<PackedScene>("res://scene/platforms/platform_collectible_single.tscn");
        _platformRowCollectable = GD.Load<PackedScene>("res://scene/platforms/platform_collectible_row.tscn");
        _platformRainbowCollectable = GD.Load<PackedScene>("res://scene/platforms/platform_collectible_rainbow.tscn");
        _platformDinoEnemy = GD.Load<PackedScene>("res://scene/platforms/platform_dino_enemy.tscn");
        _platformPlantEnemy = GD.Load<PackedScene>("res://scene/platforms/platform_plant_enemy.tscn");
        _platformCollectableAmmo = GD.Load<PackedScene>("res://scene/platforms/platform_collectible_ammo.tscn");
        _pauseMenu = GD.Load<PackedScene>("res://scene/ui/pause/menu.tscn");
        _movingEnvironment = GetNode<Node2D>("Environment/Moving");
        _player = GetNode<Player>("Player");
        _camera2D = GetNode<Camera2D>("/root/World/Camera2D");
        _collectAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Sounds/CollectSound");
        _scoreLabel = GetNode<Label>("HUD/UI/Score");
        _ammoLabel = GetNode<Label>("HUD/UI/Ammo");
        _gameOverLabel = GetNode<Label>("HUD/UI/GameOver");
        _random = new Random();
        _player.PlayerDied += OnPlayerDied;
        SpawnPlatform();
        SpawnPlatform();
        SpawnPlatform();
        SpawnPlatform();
    }

    private void OnPlayerDied()
    {
        EmitSignal(nameof(GameOver));
        _gameState.SetScore(_score);
        GetTree().ChangeSceneToFile("res://scene/ui/gameover/game_over.tscn");
    }

    public override void _Process(double delta)
    {
        if (WorldSpeed <= MaxWorldSpeed && _startTime + _timeTillSpeedUpdate < Time.GetTicksMsec())
        {
            WorldSpeed += _worldSpeedStep;
            _startTime = Time.GetTicksMsec();
        }

        if ((_resetCollectiblePitchTime + 2000) < Time.GetTicksMsec())
        {
            _collectiblePitch = 1.0f;
        }

        _scoreLabel.Text = "Score: " + _score;
        _ammoLabel.Text = "Ammo: " + _player.GetAmmo();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            _pauseMenuInstance = _pauseMenu.Instantiate<Node2D>();

            AddChild(_pauseMenuInstance);
            GetTree().Paused = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_player.Active)
        {
            return;
        }

        float newX = _movingEnvironment.Position.x - WorldSpeed * (float)delta;
        _movingEnvironment.Position = new Vector2(newX, _movingEnvironment.Position.y);
    }

    public void SpawnPlatform()
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
            platform.Position = new Vector2(600, 100);
        }
        else
        {
            float x = _lastPlatformPosition.x +
                      _random.Next(400 + (int)(WorldSpeed * 0.30), 520 + (int)(WorldSpeed * 0.40));
            float y = Math.Clamp(_lastPlatformPosition.y + _random.Next(-250, 250), -100, 580);
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
        _resetCollectiblePitchTime = Time.GetTicksMsec();
        _collectiblePitch += 0.1f;
    }

    public void ResumeGame()
    {
        RemoveChild(_pauseMenuInstance);
        _camera2D.Current = true;
        GetTree().Paused = false;
    }
}