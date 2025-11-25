using Godot;
using System;

public partial class Coin : Area2D
{
	private GameManager gameManager;
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		
		// Assumes GameManager is a sibling or parent node
		gameManager = GetNode<GameManager>("%GameManager");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	private void OnBodyEntered(Node2D body)
	{
		gameManager.AddPoints(1);
		animationPlayer.Play("pickup");
	}
}
