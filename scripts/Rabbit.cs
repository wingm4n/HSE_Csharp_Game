using Godot;
using System;
using System.Threading.Tasks;

// Класс Rabbit реализует врагов - Кроликов.
public partial class Rabbit : CharacterBody2D
{
	public bool IsInMaze = false;
	public Vector2 MazeNextPoint;
	public bool ReachedMazePoint = true;

	public static float Speed = 100.0f;
	public const float ExploisonRadius = 100.0f;
	public const int ExploisonDamage = 15;

	private bool chaseState = false;
	private bool isExploiding = false;
	private CollisionShape2D _collisionShape;

	[Export] public Player Target = new Player();
	[Export] public AnimatedSprite2D RabbitAnim;
	[Export] public Player _player;
	[Export] public Player _player2;

	public override void _Ready()
	{
		AddToGroup("enemies");

		GetNodeOrNull<Area2D>("Detector")?.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBodyEntered));
		GetNodeOrNull<Area2D>("Detector_BOOM")?.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBoomEntered));

		_player = GetTree().Root.FindChild("Player", true, false) as Player;
		_collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
		
		RabbitAnim?.Play("move");
		Target.Position = new Vector2(-3000, -3000);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isExploiding) return;

		if (IsInMaze)
		{
			float dist = Position.DistanceTo(MazeNextPoint);
			Speed = 30.0f;
			if (dist > 2.0f)
			{
				// Прямое перемещение игнорирует застревание коллизии в стенах
				Vector2 direction = (MazeNextPoint - Position).Normalized();
				Position += direction * Speed * (float)delta;
				
				ReachedMazePoint = false;
				if (RabbitAnim != null) RabbitAnim.FlipH = direction.X < 0;
			}
			else
			{
				Position = MazeNextPoint;
				ReachedMazePoint = true;
			}
			return; 
		}

		// Логика погони вне лабиринта (использует физику)
		Vector2 velocity = Velocity;
		Vector2 chaseDir = new Vector2(Math.Sign(Target.Position.X - Position.X), Math.Sign(Target.Position.Y - Position.Y));
		
		if (chaseDir != Vector2.Zero && chaseState)
		{
			velocity = chaseDir * Speed;
		}

		if (RabbitAnim != null)
		{
			RabbitAnim.FlipH = chaseDir.X < 0 || Math.Abs(Target.Position.X - Position.X) < 10;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private double DistanceToPlayer(Player body) => Math.Sqrt(Math.Pow(body.Position.X, 2) + Math.Pow(body.Position.Y, 2));

	private void OnBodyEntered(Node body)
	{
		if (body is Player p)
		{
			chaseState = true;
			if (DistanceToPlayer(p) <= DistanceToPlayer(Target)) Target = p;
		}
	}

	private void OnBoomEntered(Node body)
	{
		if (body is Player p && !isExploiding)
		{

			if (IsInMaze)
			{
				p.BunnyTakeDamage(45);
			}
			else
			{
				p.BunnyTakeDamage(10);
			}

				isExploiding = true;
			Death();
		}
	}

	public async void Death()
	{
		Player.Kills++;
		CheckKills();

		RabbitAnim?.Play("death");
		GetNodeOrNull<AudioStreamPlayer2D>("ShotSound")?.Play();
		_collisionShape?.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

		if (RabbitAnim != null) await ToSignal(RabbitAnim, "animation_finished");

		Speed += 5.0f;
		QueueFree();
	}

	private void CheckKills()
	{
		if (Player.Kills % 5 == 0)
		{
			_player?.BunnyHeal();
			_player2?.BunnyHeal();
		}
	}

	public static void ResetSpeed() => Speed = 100.0f;

	public void VarDeath(Vector2 coord, float radius)
	{
		if (coord.DistanceTo(GlobalPosition) < radius) Death();
	}
}
