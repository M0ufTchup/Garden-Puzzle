using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle.UI;

[GlobalClass]
public partial class LevelUI : Control
{
    [ExportCategory("Config")]
    [Export] private LevelModel _levelModel;
    [ExportCategory("UI")]
    [Export] private Label _moneyLabel;
    [Export] private Label _levelNameLabel;
    [ExportGroup("Win or Lose")]
    [Export] private Control _winControl;
    [Export] private Control _loseControl;

    public override void _EnterTree()
    {
        base._EnterTree();
        _levelModel.LevelStarted += OnLevelStarted;
        _levelModel.LevelEnded += OnLevelEnded;
        
        _winControl.Visible = false;
        _loseControl.Visible = false;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _levelModel.LevelStarted -= OnLevelStarted;
        _levelModel.LevelEnded -= OnLevelEnded;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _moneyLabel.Text = _levelModel.Money.ToString(); // ugly & dirty :p
    }

    private void OnLevelStarted(LevelData levelData)
    {
        _levelNameLabel.Text = _levelModel.LevelData.Name;
    }
    
    private void OnLevelEnded(LevelData obj)
    {
        Control winLoseControl = _levelModel.Won ? _winControl : _loseControl;
        winLoseControl.Visible = true;
    }
    
    private void RestartLevel() // connected through scene signals
    {
        GameInstance.Instance.LaunchLevel(_levelModel.LevelData);
    }
    
    private void ExitLevel() // connected through scene signals
    {
        GameInstance.Instance.LaunchMainMenu();
    }
}