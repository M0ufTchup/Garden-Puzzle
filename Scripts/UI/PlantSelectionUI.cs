using GardenPuzzle.Levels;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.UI;

[GlobalClass]
public partial class PlantSelectionUI : Control
{
    [Export] private LevelModel _levelModel;
    [Export] private Control _plantButtonsContainer;
    [Export] private PackedScene _plantButtonScene;

    public override void _EnterTree()
    {
        base._EnterTree();
        _levelModel.LevelStarted += OnLevelStarted;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _levelModel.LevelStarted -= OnLevelStarted;
    }

    private void OnLevelStarted(LevelData levelData)
    {
        if (levelData.AllowedPlants is null || levelData.AllowedPlants.Count == 0)
            return;
        foreach (PlantData allowedPlant in levelData.AllowedPlants)
        {
            PlantButton plantButton = _plantButtonScene.Instantiate<PlantButton>().Init(allowedPlant);
            plantButton.PlantButtonPressed += OnPlantButtonPressed;
            _plantButtonsContainer.AddChild(plantButton);
        }
    }

    private void OnPlantButtonPressed(PlantData plantData)
    {
        // TODO
    }
}