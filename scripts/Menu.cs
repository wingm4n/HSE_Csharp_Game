using Godot;
using System;
using System.Collections.Generic;

public partial class Menu : CanvasLayer
{
	private OptionButton _modeSelector;
	private OptionButton _levelSelector;
	private Button _playButton;
	private Button _quitButton;

	private const float ScaleFactor = 1.1f; 
	private const float AnimTime = 0.15f;

	private readonly Dictionary<string, string> _scenes = new()
	{
		{ "Solo_1", "res://level1.tscn" },
		{ "Solo_2", "res://level2.tscn" },
		{ "Solo_3", "res://field.tscn" },
		{ "Duo_1",  "res://level1.tscn" },
		{ "Duo_2", ""},
		{ "Duo_3",  "res://field2.tscn" }
	};

	public override void _Ready()
	{
		_modeSelector = GetNode<OptionButton>("%ModeSelector");
		_levelSelector = GetNode<OptionButton>("%LevelSelector");
		_playButton = GetNode<Button>("%PlayButton");
		_quitButton = GetNode<Button>("%QuitButton");
		
		StyleBoxFlat popupBox = new StyleBoxFlat();
		popupBox.BgColor = new Color(0.1f, 0.1f, 0.25f, 0.9f); 
		popupBox.CornerRadiusTopLeft = 30;
		popupBox.CornerRadiusBottomRight = 30;
		popupBox.CornerRadiusTopRight = 30;
		popupBox.CornerRadiusBottomLeft = 30;
		popupBox.ContentMarginLeft = 20;
		popupBox.ContentMarginRight = 20;
		popupBox.BorderWidthBottom = 5;
		popupBox.BorderWidthTop = 5;
		popupBox.BorderWidthLeft = 5;
		popupBox.BorderWidthRight = 5;
		popupBox.BorderColor = new Color(1f, 0.4f, 0.7f);

		StyleBoxFlat hoverStyle = new StyleBoxFlat();
		hoverStyle.BgColor = new Color(0.3f, 0.3f, 0.6f, 1.0f);
		hoverStyle.ContentMarginLeft = 20;
		hoverStyle.CornerRadiusTopLeft = 30;
		hoverStyle.CornerRadiusBottomRight = 30;
		hoverStyle.CornerRadiusTopRight = 30;
		hoverStyle.CornerRadiusBottomLeft = 30;
		
		_modeSelector.GetPopup().AddThemeStyleboxOverride("hover", hoverStyle);
		_levelSelector.GetPopup().AddThemeStyleboxOverride("hover", hoverStyle);
		_modeSelector.GetPopup().AddThemeStyleboxOverride("panel", popupBox);
		_levelSelector.GetPopup().AddThemeStyleboxOverride("panel", popupBox);
		_modeSelector.GetPopup().AddThemeFontSizeOverride("font_size", 40);
		_levelSelector.GetPopup().AddThemeFontSizeOverride("font_size", 40);
		
		_modeSelector.Clear();
		_modeSelector.AddItem("SOLO");
		_modeSelector.AddItem("DUO");

		_levelSelector.Clear();
		_levelSelector.AddItem("Level 1");
		_levelSelector.AddItem("Level 2");
		_levelSelector.AddItem("Level 3");

		_playButton.Pressed += OnPlayPressed;
		_quitButton.Pressed += () => GetTree().Quit();
		
		_playButton.Text = "PLAY";
		_quitButton.Text = "QUIT";

		ConnectButtonHover(_playButton);
		ConnectButtonHover(_quitButton);
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
		button.PivotOffset = button.Size / 2;

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

	private string GetKey()
	{
		string m = _modeSelector.Selected == 0 ? "Solo" : "Duo";
		int l = _levelSelector.Selected + 1;
		return $"{m}_{l}";
	}

	private void OnPlayPressed()
	{
		string key = GetKey();
		GameState.CurrentMode = _modeSelector.Selected == 0
		? GameState.Mode.Solo
		: GameState.Mode.Duo;
		if (_scenes.TryGetValue(key, out string path))
		{
			GetTree().ChangeSceneToFile(path);
		}
		else
		{
			GD.Print($"уровень не добавлен");
		}
	}
}
