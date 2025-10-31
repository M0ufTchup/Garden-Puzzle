using System.Text;
using Godot;

namespace GardenPuzzle;

[GlobalClass]
public partial class GardenLogger : Node
{
    private static GardenLogger _instance;

    private readonly StringBuilder _sb = new();
    private bool _errorRegistered = false;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        if (_instance is not null)
        {
            QueueFree();
            return;
        }

        _instance = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (_instance != this)
            return;
        _instance = null;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_sb.Length > 0)
        {
            _sb.Insert(0, Time.GetTimeStringFromSystem());
            if (_errorRegistered) GD.PrintErr(_sb.ToString());
            else GD.Print(_sb.ToString());

            _sb.Clear();
            _errorRegistered = false;
        }
    }

    public static void Log(object source, string message)
    {
        StringBuilder sb = _instance._sb;
        sb.Append('\n');
        if(sb.Length > 0) sb.Append('\t');
        sb.Append($"[{source.GetType().Name}]\t");
        sb.Append(message);
    }

    public static void LogError(object source, string message)
    {
        _instance._errorRegistered = true;
        Log(source, message);
    }
}