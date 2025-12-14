using Godot;
using System;

public partial class Menu : Node2D
{

	public override void _Ready()
	{
		Button _quit = GetNode<Button>("Quit");
		_quit.Connect(Quit.SignalName.Pressed, Callable.From(MenuQuitPressed));
		Button _play = GetNode<Button>("Play");
		_play.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlayPressed));
        Button _playDuo = GetNode<Button>("PlayDuo");
        _playDuo.Connect(Quit.SignalName.Pressed, Callable.From(MenuPlayDuoPressed));
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


}
