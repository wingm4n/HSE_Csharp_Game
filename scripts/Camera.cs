using Godot;
using System;

// Author: Korostelev A.
// Реализация камеры
public partial class Camera : Camera2D
{
	[Export] public Player _player1;
	[Export] public Player _player2;

	[Export] private float _baseZoom = 1.4f;
	[Export] private float _zoomPerDistance = -0.00045f;
	[Export] private float _minZoom = 0.55f;
	[Export] private float _maxZoom = 3.0f;
	public override void _Ready()
	{
		_player1 = GetNode<Player>($"../Player");

		_player2 = GetNodeOrNull<Player>($"../Player2");
		if (_player2 == null)
		{
			_player2 = _player1;
			_baseZoom = 1.0f;
		}
	}

	// Изменение координат и приближения камеры
	public override void _Process(double delta)
	{
		Vector2 mid = (_player1.GlobalPosition + _player2.GlobalPosition) / 2;
		GlobalPosition = GlobalPosition.Lerp(mid, (float)delta * 5f);

		float distance = _player1.GlobalPosition.DistanceTo(_player2.GlobalPosition);
		float targetZoom = _baseZoom + (distance * _zoomPerDistance);
		targetZoom = Mathf.Clamp(targetZoom, _minZoom, _maxZoom);
		Zoom = Zoom.Lerp(Vector2.One * targetZoom, (float)delta * 3f);

	}
}
