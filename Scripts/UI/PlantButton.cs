using System;
using GardenPuzzle.Plants;
using Godot;

namespace GardenPuzzle.UI;

[GlobalClass]
public partial class PlantButton : Button
{
    public event Action<PlantData> PlantButtonPressed;
    
    private PlantData _data;

    [Export] private Label _nameLabel;
    [Export] private Label _costLabel;
    
    public PlantButton Init(PlantData plantData)
    {
        _data = plantData;
        _nameLabel.Text = plantData.Name;
        _costLabel.Text = plantData.Cost.ToString();
        return this;
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        this.Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        if (_data is null)
            return;
        PlantButtonPressed?.Invoke(_data);
    }
}