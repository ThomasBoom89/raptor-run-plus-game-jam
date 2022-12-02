using Godot;

namespace RaptorRunPlus.scene;

public partial class Ground : Area2D
{
    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/World/Player");
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _player.Die();
        }
    }

    public override void _Process(double delta)
    {
    }
}