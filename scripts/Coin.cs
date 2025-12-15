using Godot;

public partial class Coin : Area2D
{
	[Export] public CoinCollector coinCollector;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;

		var sprite = GetNodeOrNull<AnimatedSprite2D>("CoinSprite");
		sprite?.Play("Rotate");
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			coinCollector?.AddCoin();
			QueueFree();
		}
	}
}
