using Godot;

namespace RaptorRunPlus.scene;

public partial class Menu : VBoxContainer
{
    private Button _startButton;

    private Button _exitButton;

    public override void _Ready()
    {
        _startButton = GetNode<Button>("StartButton");
        _exitButton = GetNode<Button>("ExitButton");
        _startButton.Pressed += OnStartButtonPressed;
        _exitButton.Pressed += OnExitButtonPressed;
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scene/world.tscn");
    }
}