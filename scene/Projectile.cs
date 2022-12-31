using Godot;

namespace RaptorRunPlus.scene;

public partial class Projectile : AnimatableBody2D
{
    private ulong _deathTime;

    public override void _Ready()
    {
        _deathTime = Time.GetTicksMsec() + 2000;
    }

    public override void _Process(double delta)
    {
        if (_deathTime < Time.GetTicksMsec())
        {
            QueueFree();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 distance = new Vector2(Vector2.Right.x * 1000 * (float)delta, 0);

        KinematicCollision2D kinematicCollision2D = MoveAndCollide(distance);
        if (kinematicCollision2D != null)
        {
            Object collider = kinematicCollision2D.GetCollider();
            if (collider.Call("is_in_group", "enemy").AsBool())
            {
                collider.Call("Hit");
            }

            QueueFree();
        }
    }
}