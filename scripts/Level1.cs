using Godot;
using System;

public partial class Level1 : Node2D
{
	private Board boardNode;
	private Godot.Label statusLabel;
	private PackedScene _pauseScene = GD.Load<PackedScene>("res://pause.tscn");

	public override void _Ready()
	{
		boardNode = GetNode<Board>("Board");
		statusLabel = GetNode<Godot.Label>("UI/StatusLabel");
		statusLabel.Text = "Mode: Solo";
	}

	public override void _Input(InputEvent @event)
	{
	if (@event is InputEventKey keyEvent && keyEvent.Pressed)
	{
		Vector2I dir = new Vector2I(0, 0);

		// solo
		if (keyEvent.Keycode == Key.W) dir = new Vector2I(0, 1);
		if (keyEvent.Keycode == Key.S) dir = new Vector2I(0, -1);
		if (keyEvent.Keycode == Key.A) dir = new Vector2I(1, 0);
		if (keyEvent.Keycode == Key.D) dir = new Vector2I(-1, 0);

		// duo
		if (keyEvent.Keycode == Key.Up) dir = new Vector2I(0, 1);
		if (keyEvent.Keycode == Key.Down) dir = new Vector2I(0, -1);
		if (keyEvent.Keycode == Key.Left) dir = new Vector2I(1, 0);
		if (keyEvent.Keycode == Key.Right) dir = new Vector2I(-1, 0);

		if (dir != new Vector2I(0, 0))
		{
			boardNode.MoveTile(dir);
		}
	}
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
