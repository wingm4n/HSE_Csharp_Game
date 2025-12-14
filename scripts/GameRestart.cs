using Godot;
using System;

public partial class GameRestart : Button
{
	public override void _Ready()
	{
	}

	public override void _Pressed()
	{
		base._Pressed();
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

}
