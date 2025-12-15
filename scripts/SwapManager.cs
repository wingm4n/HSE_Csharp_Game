using Godot;
using System;

public partial class SwapManager : Node2D
{

	private Random random;
	private double countDown = 5.0f;
	private bool swapShow = false;
	private Sprite2D _swap;

	[Export] public Player _player;
	[Export] public Player _player2;
	[Export] public Godot.Label _gameEnd;
	public override void _Ready()
	{
		random = new Random();
		_player = GetTree().Root.FindChild("Player", true, false) as Player;
		_player2 = GetTree().Root.FindChild("Player2", true, false) as Player2;
		_swap = GetTree().Root.FindChild("Swap", true, false) as Sprite2D;
		_gameEnd = GetTree().Root.FindChild("GameEnd", true, false) as Godot.Label;
	}

	public override void _Process(double delta)
	{
		if (_gameEnd.Visible)
		{
			return;
		}
		if (swapShow)
		{
			System.Threading.Thread.Sleep(1000);
			_swap.Visible = false;
			GetTree().Paused = false;
			swapShow = false;
		}
		if (countDown <= 0)
		{
			double randomNumber = random.NextDouble() * 25;
			countDown = randomNumber;
			_player.Swap = !_player.Swap;
			_player2.Swap = !_player2.Swap;
			_swap.Visible = true;
			GetTree().Paused = true;
			swapShow = true;
		}
		else
		{
			countDown -= delta;
		}


	}
}
