using Godot;
using System;

public partial class Gun : Node2D
{
	[Export] private Vector2 _offset = new Vector2(30, -7);
	private PackedScene CarrotScene = GD.Load<PackedScene>("res://carrot.tscn");
	private float _shootCooldown = 0.0f;
	private const float ShootDelay = 3.0f;
	private int _lastKills = 0;
	
	public override void _Ready()
	{
		_lastKills = Player.Kills;
	}
	
	public override void _Process(double delta)
	{
		Position = _offset;
		
		RotateToMouse();

		if (_shootCooldown > -1) {
			_shootCooldown -= (float)delta;
		}
		
		CheckKillsReset();
		
		if (Input.IsActionPressed("shoot") && (_shootCooldown <= 0))
		{
			Shoot();
			_shootCooldown = ShootDelay;
		}


	}

	private void Shoot()
	{
		if (CarrotScene != null)
		{
			var bullet = CarrotScene.Instantiate<Carrot>();
			GetParent().GetParent().AddChild(bullet);
			bullet.GlobalPosition = GlobalPosition + new Vector2((float)Math.Cos(Rotation) * 40, (float)Math.Sin(Rotation) * 40);
			bullet.Launch(new Vector2(0, -1), Rotation);
		}
	}
	
	private void CheckKillsReset()
	{
		if (Player.Kills > _lastKills)
		{
			_shootCooldown = 0.0f;
			_lastKills = Player.Kills;
		}
	}

	private void RotateToMouse()
	{
		Vector2 mousePos = GetGlobalMousePosition();
		
		Vector2 direction = (mousePos - GlobalPosition).Normalized();
		
		float angle = Mathf.Atan2(direction.Y, direction.X);
		
		Rotation = angle;
		
		if (mousePos.X < GetParent<Node2D>().GlobalPosition.X)
		{
			Scale = new Vector2(1, -1);
		}
		else
		{
			Scale = new Vector2(1, 1);
		}
	}
}
