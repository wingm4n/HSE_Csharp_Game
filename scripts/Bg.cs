using Godot;
using System;

// Author: Tyurina Z.
public partial class Bg : ParallaxBackground
{
	private float Speed = 151.0f;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		Vector2 currentOffset = ScrollOffset;

		currentOffset.X -= Speed * (float)delta;

		ScrollOffset = currentOffset;
	}
}
