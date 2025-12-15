using Godot;

public partial class CoinCollector : Node
{
	[Export] public Godot.Label coinLabel;
	private int coins = 0;

	public override void _Ready()
	{
		if (coinLabel != null)
			coinLabel.Text = "Coins: 0";
	}

	public void AddCoin()
	{
		coins++;
		if (coinLabel != null)
			coinLabel.Text = "Coins: " + coins;
	}
}
