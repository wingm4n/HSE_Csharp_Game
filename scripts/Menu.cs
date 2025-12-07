using Godot;
using System;

public partial class Menu : Node2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button _quit = GetNode<Button>("Quit");
		_quit.Connect(Quit.SignalName.Pressed, Callable.From(MenuQuitPressed));
		Button _play = GetNode<Button>("Play");
		_play.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlayPressed));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void MenuQuitPressed()
	{
		GetTree().Quit();
	}

	private void MenuPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://level.tscn");
	}



}
