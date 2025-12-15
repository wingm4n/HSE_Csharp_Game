using Godot;
using System;

public partial class CoinSpawner : Node2D
{
	[Export] public PackedScene CoinScene;
	[Export] public CoinCollector coinCollector;
	[Export] public Vector2 SpawnAreaTopLeft = new Vector2(-500, -500);
	[Export] public Vector2 SpawnAreaBottomRight = new Vector2(500, 500);

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		rng.Randomize();
		var timer = GetNode<Timer>("SpawnTimer");
		timer.Timeout += () => SpawnCoin();
	}

	private void SpawnCoin()
	{
		var coin = (Coin)CoinScene.Instantiate();
		coin.Position = new Vector2(
			rng.RandfRange(SpawnAreaTopLeft.X, SpawnAreaBottomRight.X),
			rng.RandfRange(SpawnAreaTopLeft.Y, SpawnAreaBottomRight.Y)
		);

		coin.coinCollector = coinCollector;

		AddChild(coin);
	}
}
