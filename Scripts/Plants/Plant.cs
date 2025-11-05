using GardenPuzzle.Grid;
using GardenPuzzle.MoneyGains;
using Godot;

namespace GardenPuzzle.Plants;

[GlobalClass]
public partial class Plant : Node3D
{
	private static readonly RandomNumberGenerator _rng = new();
	
	public PlantData Data { get; private set; }
	public Rect2I GridRect { get; private set; }
	public int GainedMoney { get; private set; }

	[Export] private MeshInstance3D _meshInstance; // tmp
	[Export] private float _scaleFactor = 0.75f;
	
	public void SetData(PlantData data)
	{
		Data = data;
		if (data.VisualScene is not null)
		{
			Node3D visual = data.VisualScene.Instantiate<Node3D>();
			AddChild(visual);
			visual.RotationDegrees = new Vector3(0, Data.GetUpRotationInDegrees(), 0);
			visual.Scale = (_scaleFactor * Data.CustomScaleFactor) * Vector3.One;
			_meshInstance.Visible = false;
		}
		else
		{
			_meshInstance.Mesh = Data.Mesh;
		}
	}

	public void SetGridRect(Rect2I gridRect)
	{
		GridRect = gridRect;
	}

	public int UpdateGainedMoney(IGrid grid)
	{
		GainedMoney = Data.DefaultMoneyGain;
		if(Data.AdditionalMoneyGains is null || Data.AdditionalMoneyGains.Count == 0) 
			return GainedMoney;
		
		foreach (MoneyGain additionalMoneyGain in Data.AdditionalMoneyGains)
			GainedMoney = additionalMoneyGain.ApplyMoneyGained(GainedMoney, grid, GridRect);

		return GainedMoney;
	}

	public void ClearGainedMoney()
	{
		GainedMoney = 0;
	}
}
