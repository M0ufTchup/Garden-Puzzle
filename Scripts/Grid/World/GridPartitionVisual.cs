using GardenPuzzle.Levels;
using Godot;

namespace GardenPuzzle.Grid;

[GlobalClass]
public partial class GridPartitionVisual : Node3D
{
    [Export] private Interactable _interactable;
    [Export] private Label3D _label;
    [Export] private MeshInstance3D _fogMeshInstance;
    private LevelModel _levelModel;
    private IGridPartition _gridPartition;
    
    public void Init(LevelModel levelModel, IGridPartition gridPartition)
    {
        _levelModel = levelModel;
        _gridPartition = gridPartition;

        Rect2 gridPartitionWorldRect = _levelModel.Grid.GetGridPartitionWorldRect(_gridPartition);
        Vector2 gridPartitionCenterWorldPos = gridPartitionWorldRect.GetCenter();
        GlobalPosition = new Vector3(gridPartitionCenterWorldPos.X, GlobalPosition.Y, gridPartitionCenterWorldPos.Y);
        
        BoxMesh boxMesh = _fogMeshInstance.Mesh as BoxMesh;
        boxMesh.Size = new Vector3(gridPartitionWorldRect.Size.X, boxMesh.Size.Y, gridPartitionWorldRect.Size.Y);

        _label.Text = _label.Text.Replace("[MONEY_AMOUNT]", gridPartition.Cost.ToString());

        _interactable.Enabled = _gridPartition.Locked;
        Visible = _gridPartition.Locked;
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        _interactable.InteractionCallback += OnInteractableTriggered;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (_interactable is not null) _interactable.InteractionCallback -= OnInteractableTriggered;
    }
    
    private void OnInteractableTriggered(Interactable interactable)
    {
        _levelModel.RequestGridPartitionUnlock?.Invoke(_gridPartition);
        
        _interactable.Enabled = _gridPartition.Locked;
        Visible = _gridPartition.Locked;
    }
}