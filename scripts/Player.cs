using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Movement speed and jump force
	public const float Speed = 130.0f;
	public const float JumpVelocity = -300.0f;

	// Reference to the sprite animation
	private AnimatedSprite2D _playerAnimation;

	public override void _Ready()
	{
		// Get the AnimatedSprite2D node named "playerAnimation"
		_playerAnimation = GetNode<AnimatedSprite2D>("playerAnimation");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Start with current velocity
		Vector2 velocity = Velocity;

		// Apply gravity when not on the ground
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;

		// Handle jump input
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Read input direction from keyboard (left/right)
		// Returns Vector2(-1, 0), Vector2(0, 0), or Vector2(1, 0)
		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");

		// ANIMATION LOGIC
		if (IsOnFloor())
		{
			if (inputDirection == Vector2.Zero)
				_playerAnimation.Play("idle");  // Not moving
			else
				_playerAnimation.Play("run");   // Running
		}
		else
		{
			_playerAnimation.Play("jump");      // In the air
		}

		// MOVEMENT LOGIC
		if (inputDirection != Vector2.Zero)
		{
			// Move left or right based on input
			velocity.X = inputDirection.X * Speed;

			// Flip the sprite if moving left
			_playerAnimation.FlipH = inputDirection.X < 0;
		}
		else
		{
			// Smoothly decelerate to stop
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		// Apply the updated velocity and move the character
		Velocity = velocity;
		MoveAndSlide();
	}
}
