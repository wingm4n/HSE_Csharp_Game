using Godot;
using System;

public partial class GameManager : Node
{
	public int Points = 0;
	private Label ScoreLabel;

	public override void _Ready()
	{
		ScoreLabel = GetNode<Label>("ScoreLabel");
	}

	public override void _Process(double delta)
	{
	}

	public void AddPoints(int increment)
	{
		Points += increment;
		ScoreLabel.Text = $"You collected ${Points} coins.";
	}
}
