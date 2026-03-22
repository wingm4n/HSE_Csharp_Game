using Godot;
using System;
using static Godot.TextServer;

// Author: Korostelev A. Reviewers: Svetlichny G., Kovaleva E., Tyurina Z.
// Реализация класса игрока
public partial class Player : CharacterBody2D 
{
	private PackedScene _pauseScene = GD.Load<PackedScene>("res://pause.tscn");
	
	public const float JumpVelocity = -400.0f;
	public bool Swap = false;
	private static int BunnyKills = 0;
	private int BunnyHealth = 100;
	public int Health { get { return BunnyHealth; }}
	public static int Kills { get { return BunnyKills; } set { BunnyKills = value; } }
	public AudioStreamPlayer _musicPlayer;

	public Button _gameRestart;


	public Color _colorGreen =  new Color(0, 0.6f, 0.7f, 1);
	public Color _colorWhite =  new Color(1, 1, 1, 1);
	public Color _colorYellow =  new Color(1, 0.8f, 0, 1);
	public Color _colorRed =  new Color(1, 0, 0, 1);

	[Export] public AnimatedSprite2D Bunny;
	[Export] public Node2D Gun;

	// Инициализация и подключение необходимых связей
	public override void _Ready()
	{
		_musicPlayer = GetNode<AudioStreamPlayer>($"../GameMusic");
		_musicPlayer.ProcessMode = ProcessModeEnum.Always;

		Bunny.Play("idle");
	}

	// Получение урона
	public void BunnyTakeDamage(int damage)
	{
		BunnyHealth = BunnyHealth - damage;
		if (BunnyHealth <= 0)
		{
			BunnyHealth = 0;
		}
		UpdateHPColor();
	}

	public void BunnyHeal()
	{
		BunnyHealth = BunnyHealth + 10;
		UpdateHPColor();
	}
	
	public static void ResetKills()
	{
		BunnyKills = 0;
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
	   
	}

	private static float Speed = 300.0f;
	// Обработка передвижения и выстрелов
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction;

		if (Swap)
		{
			direction = Input.GetVector("alt_left", "alt_right", "alt_up", "alt_down");
		}
		else
		{
			direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		}

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
		else
		{
			Bunny.FlipH = true;
		}

		Speed = 400 - Health;

		Velocity = velocity;
		MoveAndSlide();

	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel")) 
		{
			// если игра еще не на паузе
			if (!GetTree().Paused)
			{
				PauseGame();
			}
		}
	}
	
	private void PauseGame()
	{
		// создаем экземпляр меню паузы и добавляем его на экран
		CanvasLayer pauseInstance = _pauseScene.Instantiate<CanvasLayer>();
		AddChild(pauseInstance);
		
		// cтавим всю игру на паузу 
		GetTree().Paused = true; 
	}


}
