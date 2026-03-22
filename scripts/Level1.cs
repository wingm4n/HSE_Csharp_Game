using Godot;

public partial class Level1 : Node2D
{
	private Board boardNode;
	private Godot.Label statusLabel;
	private PackedScene _pauseScene = GD.Load<PackedScene>("res://pause.tscn");

	public override void _Ready()
	{
		_board = GetNode<Board>("Board");
		_winLabel = GetTree().Root.FindChild("WinLabel", true, false) as Godot.Label;

		if (_winLabel != null)
			_winLabel.Visible = false;

		_board.OnGameWon += HandleGameWon;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			Vector2I dir = GetDirection(keyEvent.Keycode);
			if (dir != Vector2I.Zero)
				_board.MoveTile(dir);
		}
	}

	private Vector2I GetDirection(Key key)
	{
		if (GameState.CurrentMode == GameState.Mode.Solo)
		{
			return key switch
			{
				Key.W => new Vector2I(0,  1),
				Key.S => new Vector2I(0, -1),
				Key.A => new Vector2I(1,  0),
				Key.D => new Vector2I(-1, 0),
				_     => Vector2I.Zero
			};
		}
		else
		{
			return key switch
			{
				Key.W     => new Vector2I(0,  1),
				Key.S     => new Vector2I(0, -1),
				Key.Left  => new Vector2I(1,  0),
				Key.Right => new Vector2I(-1, 0),
				_         => Vector2I.Zero
			};
		}
	}

	private void HandleGameWon()
	{
		if (_winLabel != null)
		{
			_winLabel.Text    = "YOU WIN!";
			_winLabel.Visible = true;
		}
		SetProcessInput(false);
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel")) 
		{
			// если игра еще не на паузе
			if (!GetTree().Paused)
			{
				PauseGame();
			}
		}
	}
	private void PauseGame()
	{
		// создаем экземпляр меню паузы и добавляем его на экран
		CanvasLayer pauseInstance = _pauseScene.Instantiate<CanvasLayer>();
		AddChild(pauseInstance);
		
		// cтавим всю игру на паузу 
		GetTree().Paused = true; 
	}
}
