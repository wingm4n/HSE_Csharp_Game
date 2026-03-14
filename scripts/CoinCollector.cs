using Godot;

// Author: Tyurina Z.

/// <summary>
/// Класс CoinCollector реализует подсчет собранных монет и вывод текста на экран.
/// </summary>
public partial class CoinCollector : Node
{
	[Export] public Godot.Label _coinLabel;
	[Export] public Godot.Label _gameEnd;
	[Export] public Button _gameRestart;
	private int coins = 10;

	public override void _Ready()
	{
		if (_coinLabel != null)
			_coinLabel.Text = "Coins: 0";
		_gameEnd = GetTree().Root.FindChild("GameEnd", true, false) as Godot.Label;
		_gameRestart = GetTree().Root.FindChild("GameRestart", true, false) as Button;
		_coinLabel = GetTree().Root.FindChild("CoinLabel", true, false) as Godot.Label;
	}

	public void AddCoin()
	{
		coins++;
		if (_coinLabel != null)
			_coinLabel.Text = "Coins: " + coins;

		if (coins == 5)
		{
			_gameEnd.Text = "VICTORY!";
			_gameEnd.Visible = true;
			_gameRestart.Visible = true;
			GetTree().Paused = true;
		}
	}
}
