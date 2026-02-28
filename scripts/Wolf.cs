using Godot;
using Godot.NativeInterop;
using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

// Author: Kovaleva E.

/// <summary>
/// Класс Wolf реализует врагов Волков.
/// </summary>
/// 

public partial class Wolf : CharacterBody2D
{

	// Основные данные врага

	public const float Speed = 50.0f;
	public const float ExploisonRadius = 150.0f;
	public const int ExploisonDamage = 20;

	public int WolfHealth = 20;

	private bool chaseState = false;

	private bool isExploiding = false;

	private CollisionShape2D _collisionShape;

	[Export] public AnimatedSprite2D WolfAnim;
	[Export] public Player _player;

	// Инициализирует персонажа и настраивает обработчик событий
	public override void _Ready()
	{
		AddToGroup("enemies");

		// Детектор обнаружения игрока

		Area2D _detect = GetNode<Area2D>($"./DetectorW");
		_detect.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBodyEntered));

		// Детектор вхождения в поле взрыва

		Area2D _detect_boom = GetNode<Area2D>($"./Detector_BOOMW");
		_detect_boom.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBoomEntered));

		// Находим игрока на сцене

		_player = GetTree().Root.FindChild("Player", true, false) as Player;

		// Детектор для обработки столкновений
		_collisionShape = GetNode<CollisionShape2D>($"./CollisionShape2DW");
		WolfAnim.Play("run");
	}

	// Обработка физики игры

	public override void _PhysicsProcess(double delta)
	{
		if (isExploiding) { return; }

		CharacterBody2D _player = GetNode<CharacterBody2D>($"../Player");

		Vector2 velocity = Velocity;

		// Вычисление направления движения

		Vector2 direction;
		direction.X = Math.Sign(_player.Position.X - this.Position.X);
		direction.Y = Math.Sign(_player.Position.Y - this.Position.Y);
		if ((direction != Vector2.Zero) && (chaseState))
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}

		// Отражение спрайта, если требуется

		if ((direction.X < 0) || (Math.Abs(_player.Position.X - this.Position.X) < 10))
		{
			WolfAnim.FlipH = true;
		}
		else
		{
			WolfAnim.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();

	}

	// Обработка входа тела в зону обнаружения волка.
	private void OnBodyEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body is Player)
		{
			chaseState = true;
		}
	}
	// Обработка входа тела в зону взрыва волка.
	private async void OnBoomEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body is Player)
		{

			((Player)body).BunnyTakeDamage(20);
			WolfAnim.Play("bite");
			await ToSignal(WolfAnim, "animation_finished");
			isExploiding = true;
			Death();
		}
	}

	// Метод, обрабатывающий получение урона волку
	public void WolfTakeDamage()
	{
		WolfHealth = WolfHealth - 10;
		if (WolfHealth == 0)
		{
			Death();
		}
	}
	// Метод гибели волка
	public async void Death()
	{

		WolfAnim.Play("death");
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		await ToSignal(WolfAnim, "animation_finished");
		QueueFree();
	}

	// Метод гибели врага от воздействия чего-либо
	public void VarDeath(Vector2 coord, float radius)
	{
		if (coord.DistanceTo(GlobalPosition) < radius)
		{
			this.WolfTakeDamage();
		}
	}

}
