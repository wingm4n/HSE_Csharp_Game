using Godot;
using System;

public partial class Slime : Node2D
{
	private const int Speed = 60;
	private int direction = 1;

	private RayCast2D _rayCastLeft;
	private RayCast2D _rayCastRight;
	private AnimatedSprite2D _slimeAnimation;

	public override void _Ready()
	{
		_rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		_rayCastRight = GetNode<RayCast2D>("RayCastRight");
		_slimeAnimation = GetNode<AnimatedSprite2D>("slimeAnimation");
	}

	public override void _Process(double delta)
	{
		if (_rayCastRight.IsColliding())
		{
			direction = -1;
			_slimeAnimation.FlipH = true;
		}

		if (_rayCastLeft.IsColliding())
		{
			direction = 1;
			_slimeAnimation.FlipH = false;
		}

		Position += new Vector2(direction * Speed * (float)delta, 0);
	}
}
