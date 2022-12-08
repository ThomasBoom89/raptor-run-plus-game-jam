using Godot;

namespace RaptorRunPlus.scene.camera;

public partial class CurrentFix : Camera2D
{
	public override void _Ready()
	{
		Current = true;
	}

}