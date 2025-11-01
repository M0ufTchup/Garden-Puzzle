using System;
using Godot;

namespace GardenPuzzle;

[GlobalClass]
public partial class Interactable : Node3D
{
    public event Action<Interactable> InteractionCallback;
    public bool Enabled { get; set; }

    public bool TryInteract()
    {
        if (!Enabled || InteractionCallback is null)
            return false;
        
        InteractionCallback.Invoke(this);
        return true;
    }
}