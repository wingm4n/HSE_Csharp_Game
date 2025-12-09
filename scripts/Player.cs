using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 1000.0f;
	[Export] public AnimatedSprite2D Bunny;
	public override void _Ready()
	{
		Bunny.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}
		
		if (direction.X > 0){
			Bunny.FlipH = true;
		}
		else if (direction.X < 0){
			Bunny.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
