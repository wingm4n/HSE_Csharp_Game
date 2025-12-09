using Godot;
using System;

public partial class Gun : Node2D
{
	[Export] private Vector2 _offset = new Vector2(30, -7);
    private PackedScene CarrotScene = GD.Load<PackedScene>("res://carrot.tscn");
    private float _shootCooldown = 0.0f;
    private const float ShootDelay = 5.0f;

    public override void _Process(double delta)
	{
		// 1. Следуем за игроком
		Position = _offset;
		
		// 2. Поворачиваем к курсору
		RotateToMouse();

		if (_shootCooldown > -1) {
			_shootCooldown -= (float)delta;
		}

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

    private void RotateToMouse()
	{
		Vector2 mousePos = GetGlobalMousePosition();
		
		// Вычисляем направление от оружия к курсору
		Vector2 direction = (mousePos - GlobalPosition).Normalized();
		
		// Вычисляем угол
		float angle = Mathf.Atan2(direction.Y, direction.X);
		
		// Устанавливаем поворот
		Rotation = angle;
		
		// 3. Корректируем если курсор слева
		if (mousePos.X < GetParent<Node2D>().GlobalPosition.X)
		{
			// Когда курсор слева, отражаем оружие по вертикали
			Scale = new Vector2(1, -1);
		}
		else
		{
			Scale = new Vector2(1, 1);
		}
	}
}
