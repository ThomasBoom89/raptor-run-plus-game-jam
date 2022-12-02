using Godot;
using RaptorRunPlus.scene.enemies;

namespace RaptorRunPlus.scene;

public partial class PlayArea : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("enemy"))
        {
            body.QueueFree();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("enemy"))
        {
            ((Enemy)body).SetActive();
        }
    }
}