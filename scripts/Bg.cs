using Godot;
using System;

public partial class Bg : ParallaxBackground
{
	private float Speed = 150.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 currentOffset = ScrollOffset;
		
		currentOffset.X -= Speed * (float)delta;
		
		ScrollOffset = currentOffset;
	}
}
