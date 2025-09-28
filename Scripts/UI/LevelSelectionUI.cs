using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle.UI;

[GlobalClass]
public partial class LevelSelectionUI : Control
{
    [Export] private GameConfig _gameConfig;
    [Export] private Control _levelButtonsContainer;
    [Export] private PackedScene _levelButtonScene;

    public override void _Ready()
    {
        base._Ready();
        foreach (LevelData availableLevel in _gameConfig.AvailableLevels)
        {
            LevelButton levelButton = _levelButtonScene.Instantiate<LevelButton>().Init(availableLevel);
            levelButton.LevelButtonPressed += OnLevelButtonPressed;
            _levelButtonsContainer.AddChild(levelButton);
        }
    }

    private void OnLevelButtonPressed(LevelData levelData)
    {
        GameInstance.Instance.LaunchLevel(levelData);
    }
}