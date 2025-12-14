using Godot;
using System;

public partial class LabelHP : Godot.Label

{
	[Export] Player _player;
	public override void _Ready()
	{
		_player = (Player)GetParent();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = $"HP: {_player.Health}";
		
	}
}
