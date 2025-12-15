using Godot;
using System;

public partial class Label : Godot.Label

{
	[Export] Player _player;
	public override void _Ready()
	{
		_player = GetTree().Root.FindChild("Player", true, false) as Player;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	
		Text = $"HP {_player.Health}";}
	
}
