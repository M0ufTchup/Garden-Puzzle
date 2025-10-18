using Godot;

namespace GardenPuzzle;

[GlobalClass]
public partial class GardenCamera : Camera3D
{
    private Vector3 _inputDir;
    [Export] private float _moveSpeed = 10f;
    
    public override void _Input(InputEvent inputEvent)
    {
        base._Input(inputEvent);

        _inputDir = new Vector3(
            Input.GetAxis("move_camera_left", "move_camera_right"),
            0,
            Input.GetAxis("move_camera_up", "move_camera_down"));
        if(_inputDir != Vector3.Zero)
            _inputDir = _inputDir.Normalized();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_inputDir == Vector3.Zero)
            return;
        GlobalPosition += _inputDir * (float)(_moveSpeed * delta);
    }
}