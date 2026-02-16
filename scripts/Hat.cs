using Godot;

// Author: Svetlichny G. Reviewer: Kovaleva E.
// Класс шляпы, которая появляется на поле раз в несколько
// секунд, после исчезновения которой на том же месте появляется враг

public partial class Hat : Node2D
{
	[Export] private PackedScene _rabbitScene;
	[Export] private PackedScene _wolfScene;
	[Export] private float _teleportInterval = 2.0f;
	// Возможные позиции для телепорта
	[Export]
	private Vector2[] _cornerPositions = new Vector2[]
	{
		new Vector2(-900, -300),
		new Vector2(-900, 400),
		new Vector2(800, -300),
		new Vector2(800, -400),
		new Vector2(-500, -750),
		new Vector2(-500, 900),
		new Vector2(500, -750),
		new Vector2(500, 900)
	};

	private Timer _teleportTimer;
	private bool _canSpawnRabbits = false;
	private bool _canSpawnWolfs = false;
	private Vector2 _playerStartPosition;

	public override void _Ready()
	{
		var player = GetNodeOrNull<CharacterBody2D>("/root/Field/Player");
		if (player != null)
		{
			_playerStartPosition = player.GlobalPosition;
		}

		_wolfScene = ResourceLoader.Load<PackedScene>("./wolf.tscn");

		MoveToRandomCorner();

		_teleportTimer = new Timer();
		AddChild(_teleportTimer);
		_teleportTimer.WaitTime = _teleportInterval;
		_teleportTimer.Timeout += TeleportToNewPosition;
		_teleportTimer.Start();
	}

	// Спавнит зайцев только после отхода игрока на 100 пикселей от изначальной позиции
	public override void _Process(double delta)
	{
		if (!_canSpawnRabbits)
		{
			var player = GetNodeOrNull<CharacterBody2D>("/root/Field/Player");
			if (player != null)
			{
				float distance = player.GlobalPosition.DistanceTo(_playerStartPosition);
				if (distance > 100)
				{
					_canSpawnRabbits = true;
				}
			}
		}
		if (!_canSpawnWolfs)
		{
			var player = GetNodeOrNull<CharacterBody2D>("/root/Field/Player");
			if (player != null)
			{
				float distance = player.GlobalPosition.DistanceTo(_playerStartPosition);
				if (distance > 100)
				{
					_canSpawnWolfs = true;
				}
			}
		}
	}
	// Реализация телепорта
	private void TeleportToNewPosition()
	{
		Vector2 currentPosition = GlobalPosition;

		MoveToRandomCorner();

		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		int number = rng.RandiRange(1, 4); ;

		if (_canSpawnRabbits && number < 4)
		{
			SpawnRabbitAtPosition(currentPosition);
		}
		else
		{
			SpawnWolfAtPosition(currentPosition);
		}
	}

	private void MoveToRandomCorner()
	{
		if (_cornerPositions.Length == 0) return;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		int cornerIndex = rng.RandiRange(0, _cornerPositions.Length - 1);
		GlobalPosition = _cornerPositions[cornerIndex];
	}
	// Спавн врага
	private void SpawnRabbitAtPosition(Vector2 position)
	{
		if (_rabbitScene == null) return;

		var rabbit = _rabbitScene.Instantiate<Rabbit>();

		Callable.From(() =>
		{
			GetParent().AddChild(rabbit);
			rabbit.GlobalPosition = position;
		}).CallDeferred();
	}

	private void SpawnWolfAtPosition(Vector2 position)
	{
		if (_wolfScene == null) return;

		var wolf = _wolfScene.Instantiate<Wolf>();

		Callable.From(() =>
		{
			GetParent().AddChild(wolf);
			wolf.GlobalPosition = position;
		}).CallDeferred();
	}
}
