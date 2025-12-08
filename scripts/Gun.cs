using Godot;

public partial class Gun : Node2D
{
	[Export] private Vector2 _offset = new Vector2(30, -10);
	
	public override void _Process(double delta)
	{
		// 1. Следуем за игроком
		Position = _offset;
		
		// 2. Поворачиваем к курсору
		RotateToMouse();
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
