using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle;

/// <summary>
/// Singleton will be existing from the start to the end of the application. Handles the logic of the app as a whole (UnrealEngine's GameInstance like)
/// </summary>
[GlobalClass]
public partial class GameInstance : Node
{
    public static GameInstance Instance { get; private set; }

    [Export] private PackedScene _mainMenuScene;
    [Export] private PackedScene _levelManagerScene;

    public override void _EnterTree()
    {
        base._EnterTree();
        if (Instance is not null && Instance != this)
        {
            GardenLogger.LogError(this, "Cannot have multiple game instances");
            QueueFree();
            return;
        }
        Instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if(Instance == this)
            Instance = null;
    }

    public void LaunchLevel(LevelData levelData)
    {
        LevelManager levelManager = _levelManagerScene.Instantiate<LevelManager>();
        ChangeSceneToNode(levelManager).Init(levelData);
    }
    public void LaunchMainMenu() => ChangeSceneToNode(_mainMenuScene.Instantiate());

    private T ChangeSceneToNode<T>(T node) where T : Node
    {
        var tree = GetTree();
        var currentScene = tree.GetCurrentScene();
        tree.GetRoot().RemoveChild(currentScene);
        tree.GetRoot().AddChild(node);
        tree.SetCurrentScene(node);
        return node;
    }
}