using Godot;
using System;

public partial class Carrot : Area2D
{
	[Export] public float Speed { get; set; } = 300.0f;
	private Rect2 gamefieldBounds = new Rect2(-2000, -2000, 5000, 5000);

	private Vector2 _direction = Vector2.Up;

	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	public void Launch(Vector2 direction, float rotation)
	{
		_direction.X = (float)Math.Cos(rotation);
		_direction.Y = (float)Math.Sin(rotation);
		Rotation = rotation + (float)Math.PI/2;
	}

	public override void _Process(double delta)
	{
		GlobalPosition += _direction * Speed * (float)delta;

		if (!gamefieldBounds.HasPoint(GlobalPosition))
		{
			QueueFree();
		}
	}

	private async void OnBodyEntered(Node2D body)
	{
		if (body is Rabbit)
		{
			foreach (Node enemy in GetTree().GetNodesInGroup("enemies"))
			{
				if (enemy is Rabbit enemyNode)
				{
					enemyNode.VarDeath(GlobalPosition, ExplosionRadius);
				}
			}
			BoomAnim = GetNode<AnimatedSprite2D>("BoomAnim");
			Sprite2D NoAnim = GetNode<Sprite2D>("Sprite2D");
			NoAnim.Visible = false;
			BoomAnim.Play("boom");
			BoomAnim.Visible = true;
			stopped = true;

			await ToSignal(BoomAnim, "animation_finished");
			QueueFree();
		}
	}
}
