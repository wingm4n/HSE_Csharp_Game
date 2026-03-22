using Godot;
using System;

// Author: Korostelev A.
public partial class Menu : Node2D
{

	public override void _Ready()
	{
		Button _quit = GetNode<Button>("Quit");
		_quit.Connect(Quit.SignalName.Pressed, Callable.From(MenuQuitPressed));
		Button _play = GetNode<Button>("PlaySolo3");
		_play.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlayPressed));
		Button _playDuo = GetNode<Button>("PlayDuo3");
		_playDuo.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlayDuoPressed));
		Button _play1 = GetNode<Button>("PlaySolo1");
		_play1.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlay1Pressed));
		Button _play2 = GetNode<Button>("PlaySolo2");
		_play2.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlay2Pressed));
	}

	public override void _Process(double delta)
	{
	}

	private void MenuQuitPressed()
	{
		GetTree().Quit();
	}

	private void MenuPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://field.tscn");
	}

	private void MenuPlayDuoPressed()
	{
		GetTree().ChangeSceneToFile("res://field2.tscn");
	}
	
	private void MenuPlay1Pressed()
	{
		GetTree().ChangeSceneToFile("res://level1.tscn");
	}
	
	private void MenuPlay2Pressed()
	{
		GetTree().ChangeSceneToFile("res://level2.tscn");
	}


}
