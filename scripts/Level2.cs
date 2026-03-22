using Godot;
using System;
using System.Collections.Generic;

public class LabyrinthGenerator
{
	const int blankN = 0;
	const int wallN = 1;

	public int[,] Generate(int fieldN)
	{
		int[,] map = new int[fieldN, fieldN];
		Random rnd = new Random();

		for (int i = 0; i < fieldN; i++)
			for (int j = 0; j < fieldN; j++)
				map[i, j] = wallN;

		int si = rnd.Next(0, fieldN / 2) * 2 + 1;
		int sj = rnd.Next(0, fieldN / 2) * 2 + 1;
		map[si, sj] = blankN;

		int[] di = { -2, 0, 0, +2 };
		int[] dj = { 0, -2, +2, 0 };

		while (true)
		{
			bool ready = true;
			for (int i = 1; i < fieldN; i += 2)
				for (int j = 1; j < fieldN; j += 2)
					if (map[i, j] != blankN) ready = false;

			if (ready) break;

			while (true)
			{
				int dir = rnd.Next(0, 4);
				bool fDeadEnd = true;

				for (int j = 0; j < 4; j++)
				{
					int ki = si + di[j];
					int kj = sj + dj[j];
					if (!((ki <= 0) || (kj <= 0) || (ki >= fieldN) || (kj >= fieldN) || (map[ki, kj] == blankN)))
						fDeadEnd = false;
				}

				if (fDeadEnd)
				{
					si = rnd.Next(0, fieldN / 2) * 2 + 1;
					sj = rnd.Next(0, fieldN / 2) * 2 + 1;
					while (map[si, sj] != 0)
					{
						si = rnd.Next(0, fieldN / 2) * 2 + 1;
						sj = rnd.Next(0, fieldN / 2) * 2 + 1;
					}
					map[si, sj] = blankN;
					break;
				}

				int ni = si + di[dir];
				int nj = sj + dj[dir];

				if ((ni <= 0) || (nj <= 0) || (ni >= fieldN) || (nj >= fieldN) || (map[ni, nj] == blankN)) continue;

				map[ni, nj] = blankN;
				map[si + di[dir] / 2, sj + dj[dir] / 2] = blankN;
				si = ni;
				sj = nj;
			}
		}
		return map;
	}
}

public partial class Level2 : Node2D
{
	[Export] private TileMapLayer _tileMap;
	[Export] public PackedScene RabbitScene;
	[Export] public Player _player;
	[Export] public Node2D _key;

	public int[,] _labyrinthMap;
	private int _mazeWidth = 21;
	private Vector2I keyPos;
	private Vector2I gatePos;
	private bool keyReady = false;

	private List<Rabbit> _rabbits = new List<Rabbit>();
	private List<Vector2I> _rabbitDirections = new List<Vector2I>();

	public override void _Ready()
	{
		_tileMap = GetNode<TileMapLayer>("Node2D/Layer0");
		_tileMap.TileSet.TileSize = new Vector2I(16, 16);
		_player = GetTree().Root.FindChild("Player", true, false) as Player;
		_key = GetTree().Root.FindChild("Key", true, false) as Node2D;
		CallDeferred(nameof(GenerateLabyrinth), _mazeWidth);
	}

	public void GenerateLabyrinth(int width)
	{
		var generator = new LabyrinthGenerator();
		// _labyrinthMap = generator.Generate(width);
		var map = generator.Generate(width);
		var map2 = generator.Generate(width + 2);
		for (int i = 0; i < width - 1; i++)
		{
			for (int j = 0; j < width - 1; j++)
			{
				map[i, j] = map[i, j] * map2[i, j];
			}
		}
		_labyrinthMap = map;

		_tileMap.Clear();
		List<Vector2I> floorCells = new List<Vector2I>();
		Random rnd = new Random();

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < width; y++)
			{
				if (_labyrinthMap[x, y] == 1) 
					_tileMap.SetCell(new Vector2I(x, y), 2, new Vector2I(rnd.Next(0, 4), rnd.Next(0,4)));
				else 
				{
					_tileMap.SetCell(new Vector2I(x, y), 0, new Vector2I(15, 15));
					floorCells.Add(new Vector2I(x, y));
				}
			}
		}

		_tileMap.CollisionEnabled = true;

		if (RabbitScene != null && floorCells.Count > 5)
		{
			for (int i = 0; i < 10; i++)
			{
				int idx = rnd.Next(floorCells.Count);
				Vector2I cell = floorCells[idx];
				floorCells.RemoveAt(idx);

				Rabbit r = RabbitScene.Instantiate<Rabbit>();
				_tileMap.AddChild(r); 
				
				r.Scale = Vector2.One / _tileMap.Scale;

				Vector2 spawnPos = _tileMap.MapToLocal(cell);
				r.Position = spawnPos;
				r.MazeNextPoint = spawnPos;
				r.IsInMaze = true;
				r.ReachedMazePoint = true; 

				_rabbits.Add(r);
				_rabbitDirections.Add(Vector2I.Zero); // На старте направления нет
			}
		}

		// Add key
		int si = rnd.Next(0, width / 2) * 2 + 1;
		int sj = rnd.Next(0, width / 2) * 2 + 1;
		while (_labyrinthMap[si, sj] != 0)
		{
			si = rnd.Next(0, width / 2) * 2 + 1;
			sj = rnd.Next(0, width / 2) * 2 + 1;
		}
		_labyrinthMap[si, sj] = 2;
		keyPos = new Vector2I(si, sj);
		_tileMap.SetCell(keyPos, 8, new Vector2I(0, 0));

		// Add exit
		si = rnd.Next(0, width / 2) * 2 + 1;
		sj = rnd.Next(0, width / 2) * 2 + 1;
		while (_labyrinthMap[si, sj] != 0)
		{
			si = rnd.Next(0, width / 2) * 2 + 1;
			sj = rnd.Next(0, width / 2) * 2 + 1;
		}
		_labyrinthMap[si, sj] = 3;
		gatePos = new Vector2I(si, sj);
		_tileMap.SetCell(gatePos, 3, new Vector2I(0, 3));
	}

	public override void _PhysicsProcess(double delta)
	{

		Vector2 globalKeyPos = _tileMap.ToGlobal(_tileMap.MapToLocal(keyPos));
		if (globalKeyPos.DistanceTo(_player.Position) < 30)
		{
			_key.Visible = true;
			keyReady = true;
			_labyrinthMap[keyPos.X, keyPos.Y] = 0;
			_tileMap.SetCell(keyPos, 0, new Vector2I(15, 15));

		}

		Vector2 globalGatePos = _tileMap.ToGlobal(_tileMap.MapToLocal(gatePos));
		if (globalGatePos.DistanceTo(_player.Position) < 30 && keyReady)
		{
			GD.Print("WIN WIN");
		}

		for (int i = _rabbits.Count - 1; i >= 0; i--)
		{
			Rabbit r = _rabbits[i];
			if (!IsInstanceValid(r)) 
			{ 
				_rabbits.RemoveAt(i); 
				_rabbitDirections.RemoveAt(i); 
				continue; 
			}

			if (r.ReachedMazePoint)
			{
				Vector2I curr = _tileMap.LocalToMap(r.Position);
				Vector2I[] dirs = { Vector2I.Up, Vector2I.Down, Vector2I.Left, Vector2I.Right };
				List<Vector2I> valid = new List<Vector2I>();

				foreach (var d in dirs)
				{
					Vector2I n = curr + d;
					if (n.X >= 0 && n.X < _mazeWidth && n.Y >= 0 && n.Y < _mazeWidth)
					{
						if (_labyrinthMap[n.X, n.Y] == 0) valid.Add(d);
					}
				}

				if (valid.Count > 0)
				{
					// Убираем разворот назад, если есть выбор
					if (valid.Count > 1 && _rabbitDirections[i] != Vector2I.Zero)
					{
						valid.Remove(-_rabbitDirections[i]);
					}

					_rabbitDirections[i] = valid[GD.RandRange(0, valid.Count - 1)];
					r.MazeNextPoint = _tileMap.MapToLocal(curr + _rabbitDirections[i]);
					r.ReachedMazePoint = false;
				}
			}
		}
	}
}
