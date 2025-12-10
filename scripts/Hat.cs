using Godot;

public partial class Hat : Node2D
{
	[Export] private PackedScene _rabbitScene;
	[Export] private float _teleportInterval = 3.0f;
	
	[Export] private Vector2[] _cornerPositions = new Vector2[]
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
	private Vector2 _playerStartPosition;
	
	public override void _Ready()
	{
		var player = GetNodeOrNull<CharacterBody2D>("/root/Field/Player");
		if (player != null)
		{
			_playerStartPosition = player.GlobalPosition;
		}
		
		MoveToRandomCorner();
		
		_teleportTimer = new Timer();
		AddChild(_teleportTimer);
		_teleportTimer.WaitTime = _teleportInterval;
		_teleportTimer.Timeout += TeleportToNewPosition;
		_teleportTimer.Start();
	}
	
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
	}
	
	private void TeleportToNewPosition()
	{
		Vector2 currentPosition = GlobalPosition;
		
		MoveToRandomCorner();
		
		if (_canSpawnRabbits)
		{
			SpawnRabbitAtPosition(currentPosition);
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
	
	private void SpawnRabbitAtPosition(Vector2 position)
	{
		if (_rabbitScene == null) return;
		
		var rabbit = _rabbitScene.Instantiate<Rabbit>();
		
		Callable.From(() => {
			GetParent().AddChild(rabbit);
			rabbit.GlobalPosition = position;
		}).CallDeferred();
	}
}
