using Godot;

public partial class Level1 : Node2D
{
	private Board _board;
	private Godot.Label _winLabel;

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
}
