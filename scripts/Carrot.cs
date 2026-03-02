using Godot;
using System;

// Author: Korostelev A.
// Реализация игрового снаряда - морковки
public partial class Carrot : Area2D
{
	[Export] public float _speed { get; set; } = 501.0f;
	[Export] public AnimatedSprite2D _boomAnim;
    [Export] public Sprite2D _noAnim;
    private Rect2 gamefieldBounds = new Rect2(-2000, -2000, 5000, 5000);
	private bool Stopped = false;

	private Vector2 _direction = Vector2.Up;
	public float ExplosionRadius { get; set; } = 200.0f;

	// Инициализация
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        _boomAnim = GetNode<AnimatedSprite2D>("BoomAnim");
        _noAnim = GetNode<Sprite2D>("Sprite2D");
    }
	public void Launch(Vector2 direction, float rotation)
	{
		_direction.X = (float)Math.Cos(rotation);
		_direction.Y = (float)Math.Sin(rotation);
		Rotation = rotation + (float)Math.PI/2;
	}

	// Обработка полета морковки
	public override void _Process(double delta)
	{
		if (!Stopped)
		{
			GlobalPosition += _direction * _speed * (float)delta;
		}

		if (!gamefieldBounds.HasPoint(GlobalPosition))
		{
			QueueFree();
		}
	}

	// Обработка столкновений
	private async void OnBodyEntered(Node2D body)
	{
		if (body is Rabbit || body is Wolf)
		{
			foreach (Node enemy in GetTree().GetNodesInGroup("enemies"))
			{
				if (enemy is Rabbit enemyNode)
				{
					enemyNode.VarDeath(GlobalPosition, ExplosionRadius);
				}
                if (enemy is Wolf enemyNode2)
                {
                    enemyNode2.VarDeath(GlobalPosition, ExplosionRadius);
                }
            }


			_noAnim.Visible = false;
			_boomAnim.Play("boom");
			_boomAnim.Visible = true;
			Stopped = true;

			await ToSignal(_boomAnim, "animation_finished");
			QueueFree();
		}
	}
}
