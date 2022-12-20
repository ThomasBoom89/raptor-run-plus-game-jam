using Godot;
using RaptorRunPlus.scene.service;

namespace RaptorRunPlus.scene.ui.gameover;

public partial class GameOver : Node2D
{
    private GameState _gameState;
    private Highscore _highscore;
    private Label _yourScoreLabel;
    private Label _rankLabel;
    private Button _retryButton;
    private Button _backButton;
    private Button _quitButton;
    private Label _notGoodEnough;
    private VBoxContainer _submitHighscore;
    private VBoxContainer _submitContainer;
    private LineEdit _name;
    private Button _submit;
    private HighscoreBoard _highscoreBoard;

    public GameOver()
    {
        _gameState = DIContainer.GetFactory().GetGameState();
        _highscore = DIContainer.GetFactory().GetHighscore();
    }

    public override void _Ready()
    {
        _retryButton = GetNode<Button>("%Retry");
        _backButton = GetNode<Button>("%Back");
        _quitButton = GetNode<Button>("%Quit");
        _submitHighscore = GetNode<VBoxContainer>("%SubmitHighscore");
        _submitContainer = GetNode<VBoxContainer>("%SubmitContainer");
        _name = GetNode<LineEdit>("%Name");
        _submit = GetNode<Button>("%Submit");
        _rankLabel = GetNode<Label>("%Rank");
        _notGoodEnough = GetNode<Label>("%NotGoodEnough");
        _yourScoreLabel = GetNode<Label>("%YourScore");
        _highscoreBoard = GetNode<HighscoreBoard>("%HighscoreBoard");

        _retryButton.Pressed += OnRetryButtonPressed;
        _backButton.Pressed += OnBackButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;

        _yourScoreLabel.Text = "Your score: " + _gameState.GetScore();
        _retryButton.GrabFocus();
        _initializeSubmitHighscore();
    }

    private void _initializeSubmitHighscore()
    {
        int score = _gameState.GetScore();
        int rank = _highscore.GetRankByScore(score);
        if (rank > 0)
        {
            _submit.Pressed += OnSubmitButtonPressed;
            _rankLabel.Text = "Your rank: " + rank + "!";
            _submitHighscore.Visible = true;
            _name.GrabFocus();
        }
        else
        {
            _notGoodEnough.Visible = true;
        }
    }

    private void OnSubmitButtonPressed()
    {
        if (_name.Text.Length <= 0)
        {
            return;
        }

        _submitContainer.Visible = false;
        _highscore.InsertEntry(_name.Text, _gameState.GetScore());
        _highscoreBoard.Refresh();
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scene/menu.tscn");
    }

    private void OnRetryButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scene/world.tscn");
    }
}