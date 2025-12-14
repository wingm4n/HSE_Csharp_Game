using Godot;
using System;

public partial class Player2 : Player
{
	private float Speed = 450.0f;
	private float _dashCooldown = 0.0f;
	private const float DashDelay = 3.0f;
	private const float NormalSpeed = 450.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("alt_left", "alt_right", "alt_up", "alt_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
			Bunny.Play("move");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			Bunny.Play("idle");
		}

		if (direction.X < 0)
		{
			Bunny.FlipH = false;
		}
		else
		{
			Bunny.FlipH = true;
		}

		if (_dashCooldown > -1)
		{
			_dashCooldown -= (float)delta;
		}

		if (_dashCooldown <= DashDelay/2)
		{
			Speed = NormalSpeed;
		}


		if (Input.IsActionPressed("dash") && (_dashCooldown <= 0))
		{
			Speed = NormalSpeed * 2;
			_dashCooldown = DashDelay;
		}

		Velocity = velocity;
		MoveAndSlide();

	}

}
