using Godot;
using System;

public partial class Tile : Node2D
{
	public int Number;            
	public Vector2I GridPos;      
	public bool IsEmpty = false;

	private Godot.ColorRect rect;
	private Godot.Label label;

	public override void _Ready()
	{
		rect = new Godot.ColorRect();
		rect.Size = new Vector2(100, 100);
		rect.Color = IsEmpty ? new Color(0,0,0,0) : new Color(0.2f,0.6f,1f);
		AddChild(rect);

		label = new Godot.Label();
		label.Text = IsEmpty ? "" : Number.ToString();
		label.Position = new Vector2(35,30);
		AddChild(label);
	}

	public void UpdatePosition(Vector2I newGridPos)
	{
		GridPos = newGridPos;
		Position = new Vector2(GridPos.X * 110, GridPos.Y * 110);
	}
}
