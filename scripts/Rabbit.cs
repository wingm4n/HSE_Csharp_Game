using Godot;
using System;

public partial class Rabbit : CharacterBody2D
{
	public const float Speed = 100.0f;

	private bool chaseState = false;
	[Export] public AnimatedSprite2D RabbitAnim;
	public override void _Ready()
	{
		Area2D _detect = GetNode<Area2D>($"./Detector");
		_detect.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBodyEntered));
		RabbitAnim.Play("move");
	}
	public override void _PhysicsProcess(double delta)
	{
		CharacterBody2D _player = GetNode<CharacterBody2D>($"../Player");
		Vector2 velocity = Velocity;

		Vector2 direction;
		direction.X = Math.Sign(_player.Position.X - this.Position.X);
		direction.Y = Math.Sign(_player.Position.Y - this.Position.Y);
		if ((direction != Vector2.Zero) && (chaseState))
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}

		if ((direction.X< 0) || (Math.Abs(_player.Position.X - this.Position.X) < 10))
		{
			RabbitAnim.FlipH = true;
		}
		else
		{
			RabbitAnim.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnBodyEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body.Name == "Player")
		{
			chaseState = true;
		}
	}

}
