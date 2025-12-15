using Godot;

public partial class CoinCollector : Node
{
	[Export] public Godot.Label coinLabel;
    [Export] public Godot.Label _gameEnd;
    public Button _gameRestart;
    private int coins = 0;

	public override void _Ready()
	{
		if (coinLabel != null)
			coinLabel.Text = "Coins: 0";
        _gameEnd = GetTree().Root.FindChild("GameEnd", true, false) as Godot.Label;
        _gameRestart = GetTree().Root.FindChild("GameRestart", true, false) as Button;
    }

	public void AddCoin()
	{
		coins++;
		if (coinLabel != null)
			coinLabel.Text = "Coins: " + coins;
		if (coins == 1)
		{
			_gameEnd.Text = "VICTORY!";
            _gameEnd.Visible = true;
            _gameRestart.Visible = true;
            GetTree().Paused = true;
        }
	}
}
