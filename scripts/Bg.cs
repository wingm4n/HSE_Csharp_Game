using Godot;
using System;

// Author: Tyurina Z.
public partial class Bg : ParallaxBackground
{
	private float Speed = 150.0f;

	public override void _Ready()
	{
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		foreach (ParallaxLayer layer in GetChildren())
		{
			Sprite2D sprite = (Sprite2D)layer.GetChild(0);
			float scale = screenSize.Y / sprite.Texture.GetSize().Y;
			sprite.Scale = new Vector2(scale, scale);
			layer.MotionMirroring = new Vector2(sprite.Texture.GetSize().X * scale, 0);
		}
	}

	public override void _Process(double delta)
	{
		ScrollOffset = new Vector2(ScrollOffset.X - Speed * (float)delta, 0);
	}
}
