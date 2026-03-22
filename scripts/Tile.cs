using Godot;
using System;

public partial class Tile : Node2D
{
	public const int TileSize = 400;
	public const int TileGap  = 10;
	public const int TileStep = TileSize + TileGap;

	public int Number;
	public Vector2I GridPos;
	public bool IsEmpty = false;

	private static readonly Color ColorNormal = new Color(0.6f, 0.82f, 0.94f);
	private static readonly Color ColorEmpty  = new Color(0, 0, 0, 0);
	private static readonly Color ColorText   = new Color(1f, 1f, 1f);

	public override void _Ready()
	{
		var panel = new Panel();
		panel.Size = new Vector2(TileSize, TileSize);

		var style = new StyleBoxFlat();
		style.BgColor = IsEmpty ? ColorEmpty : ColorNormal;
		style.CornerRadiusTopLeft     = 12;
		style.CornerRadiusTopRight    = 12;
		style.CornerRadiusBottomLeft  = 12;
		style.CornerRadiusBottomRight = 12;
		panel.AddThemeStyleboxOverride("panel", style);
		AddChild(panel);

		var label = new Label();
		label.Text = IsEmpty ? "" : Number.ToString();
		label.Size = new Vector2(TileSize, TileSize);
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment   = VerticalAlignment.Center;
		label.AddThemeColorOverride("font_color", ColorText);
		label.AddThemeFontSizeOverride("font_size", 120);
		label.AddThemeConstantOverride("outline_size", 3);
		label.AddThemeColorOverride("font_outline_color", new Color(0f, 0f, 0f));
		AddChild(label);
	}

	public void UpdatePosition(Vector2I newGridPos)
	{
		GridPos  = newGridPos;
		Position = new Vector2(GridPos.X * TileStep, GridPos.Y * TileStep);
	}
}
