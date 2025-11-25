using Godot;
using System;

public partial class Killzone : Area2D
{
	private Timer _timer;

	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");

		BodyEntered += OnBodyEntered;
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("You died!");
		Engine.TimeScale = 0.5;
		body.GetNode<CollisionShape2D>("PlayerCollisionShape2D").QueueFree();
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		Engine.TimeScale = 1;
		GetTree().ReloadCurrentScene();
	}
}
