using Godot;
using System;

public partial class Pause : CanvasLayer
{
	private const float ScaleFactor = 1.1f; 
	private const float AnimTime = 0.15f;
	
	private Button _toMenu;
	private Button _continue;
	private Button _restart;
	private Godot.Label _statusLabel;
	
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		_toMenu = GetNode<Button>("VBoxContainer/ToMenu");
		_continue = GetNode<Button>("VBoxContainer/Continue");
		_restart = GetNode<Button>("VBoxContainer/Restart");
		_statusLabel = GetNode<Godot.Label>("VBoxContainer/StatusLabel");
		
		_toMenu.Connect(Button.SignalName.Pressed, Callable.From(ToMenuPressed));
		_continue.Connect(Button.SignalName.Pressed, Callable.From(ContinuePressed));
		_restart.Connect(Button.SignalName.Pressed, Callable.From(RestartPressed));
		
		ConnectButtonHover(_continue);
		ConnectButtonHover(_restart);
		ConnectButtonHover(_toMenu);
	}
	
	public void SetVictoryMode()
	{
		if (_continue == null) _continue = GetNode<Button>("VBoxContainer/Continue");
		if (_statusLabel == null) _statusLabel = GetNode<Godot.Label>("VBoxContainer/StatusLabel");
		
		_continue.Visible = false;      
		_statusLabel.Text = "VICTORY!"; 
		_statusLabel.Visible = true;    
		
		_statusLabel.AddThemeColorOverride("font_color", new Color(0.2f, 1.0f, 0.2f)); 
		_statusLabel.AddThemeFontSizeOverride("font_size", 80); 
		_statusLabel.HorizontalAlignment = HorizontalAlignment.Center; 
	}

	public void SetGameOverMode()
	{
		if (_continue == null) _continue = GetNode<Button>("VBoxContainer/Continue");
		if (_statusLabel == null) _statusLabel = GetNode<Godot.Label>("VBoxContainer/StatusLabel");
		
		_continue.Visible = false;       
		_statusLabel.Text = "DEFEAT!"; 
		_statusLabel.Visible = true;     
		
		_statusLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.2f, 0.2f)); 
		_statusLabel.AddThemeFontSizeOverride("font_size", 80); 
		_statusLabel.HorizontalAlignment = HorizontalAlignment.Center; 
	}
	
	private void ConnectButtonHover(Button button)
	{
		button.SetMeta("no_container_sizing", true);
		button.PivotOffset = button.Size / 2;
		button.Connect(Button.SignalName.MouseEntered, Callable.From(() => OnButtonHovered(button)));
		button.Connect(Button.SignalName.MouseExited, Callable.From(() => OnButtonUnhovered(button)));
		button.Connect(Button.SignalName.FocusEntered, Callable.From(() => OnButtonHovered(button)));
		button.Connect(Button.SignalName.FocusExited, Callable.From(() => OnButtonUnhovered(button)));
	}

	private void OnButtonHovered(Button button)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(button, "scale", new Vector2(ScaleFactor, ScaleFactor), AnimTime)
			 .SetTrans(Tween.TransitionType.Quad)
			 .SetEase(Tween.EaseType.Out);        
	}

	private void OnButtonUnhovered(Button button)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(button, "scale", Vector2.One, AnimTime)
			 .SetTrans(Tween.TransitionType.Quad)
			 .SetEase(Tween.EaseType.In);
	}

	private void ToMenuPressed()
	{
		GetTree().Paused = false; 
		Player.ResetKills();
		Rabbit.ResetSpeed();
		GetTree().ChangeSceneToFile("res://menu.tscn"); 
	}

	private void ContinuePressed()
	{
		GetTree().Paused = false;
		QueueFree(); 
	}
	
	private void RestartPressed()
	{
		GetTree().Paused = false;
		Player.ResetKills();
		Rabbit.ResetSpeed();
		GetTree().ReloadCurrentScene();
	}
}
