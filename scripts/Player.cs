using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public static float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public static int Kills = 0;
	public int BunnyHealth = 100;
	private AudioStreamPlayer _musicPlayer;
	private Godot.Label _hpLabel;
	private Timer _colorResetTimer; 
	private Godot.Label _gameEnd;

	private Color _colorGreen =  new Color(0, 1, 0, 1);
	private Color _colorWhite =  new Color(1, 1, 1, 1);
	private Color _colorYellow =  new Color(1, 1, 0, 1);
	private Color _colorRed =  new Color(1, 0, 0, 1);

	[Export] public AnimatedSprite2D Bunny;
	[Export] public Node2D Gun;
	
	public override void _Ready()
	{
		_musicPlayer = GetNode<AudioStreamPlayer>($"../GameMusic");
		_musicPlayer.ProcessMode = ProcessModeEnum.Always;
		_gameEnd = GetNode<Godot.Label>($"./GameEnd");
		_hpLabel = GetNode<Godot.Label>($"./LabelHP");
		Bunny.Play("idle");
	}

	public void BunnyTakeDamage()
	{
		BunnyHealth = BunnyHealth - 10;
		if (BunnyHealth <= 0)
		{
			BunnyHealth = 0;
			_gameEnd.Visible = true;
			GetTree().Paused = true;
		}
		Speed = 400 - BunnyHealth;
		UpdateHPColor();
	}

	public void BunnyHeal()
	{
		BunnyHealth = BunnyHealth + 10;
		UpdateHPColor();
	}
	
	private void UpdateHPColor()
	{
		Color targetColor;
		
		if (BunnyHealth > 100)
		{
			targetColor = _colorGreen;
		}
		else if (BunnyHealth >= 60)
		{
			targetColor = _colorWhite;
		}
		else if (BunnyHealth >= 30)
		{
			targetColor = _colorYellow;
		}
		else
		{
			targetColor = _colorRed;
		}
	   
		_hpLabel.AddThemeColorOverride("font_color", targetColor);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
			Bunny.Play("move");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			Bunny.Play("idle");
		}

		if (direction.X < 0)
		{
			Bunny.FlipH = false;
		}
		else {
			Bunny.FlipH = true;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
