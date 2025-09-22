using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GameGrid : Node3D
{
	private const float RayLength = 1000.0f;
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
		{
            GetObjectUnderMouse(eventMouseButton.Position);
		}
	}

	private void GetObjectUnderMouse(Vector2 MousePosition)
	{
        PhysicsDirectSpaceState3D space = GetWorld3D().DirectSpaceState;
        Camera3D camera3D = GetViewport().GetCamera3D();
        Vector3 from = camera3D.ProjectRayOrigin(MousePosition);
        Vector3 to = from + camera3D.ProjectRayNormal(MousePosition) * RayLength;

        PhysicsRayQueryParameters3D qparams = new PhysicsRayQueryParameters3D();
        qparams.From = from;
        qparams.To = to;

        Dictionary result = space.IntersectRay(qparams);
        if (result.Count <= 0)
            return;

        GD.Print(result);
    }
}
