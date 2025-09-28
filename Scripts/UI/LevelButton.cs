using System;
using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle.UI;

[GlobalClass]
public partial class LevelButton : Button
{
    public event Action<LevelData> LevelButtonPressed;
    private LevelData _levelData;
    
    [Export] private Label _levelNameLabel;
    
    public LevelButton Init(LevelData levelData)
    {
        _levelData = levelData;
        _levelNameLabel.Text = _levelData.Name;
        this.Pressed += OnPressed;
        return this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (_levelData is not null)
            this.Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        LevelButtonPressed?.Invoke(_levelData);
    }
}