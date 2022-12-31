using Godot;

namespace RaptorRunPlus.scene.ui.pause;

public partial class Menu : VBoxContainer
{
    private Button _resume;

    private Button _backToMenu;

    private Button _quit;

    private Game _game;

    public override void _Ready()
    {
        _game = GetNode<Game>("/root/World");
        _resume = GetNode<Button>("ResumeButton");
        _backToMenu = GetNode<Button>("BackToMenuButton");
        _quit = GetNode<Button>("QuitButton");

        _resume.Pressed += OnResumePressed;
        _backToMenu.Pressed += OnBackToMenuPressed;
        _quit.Pressed += OnQuitPressed;
        _resume.GrabFocus();
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }

    private void OnBackToMenuPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scene/menu.tscn");
    }

    private void OnResumePressed()
    {
        _game.ResumeGame();
    }
}