using Godot;
using System;

public class LabyrinthGenerator
{
	const int blankN = 0;
	const int wallN = 1;
	public int[,] Generate(int fieldN)
	{
		int[,] map = new int[fieldN, fieldN];
		Random rnd = new Random();

			for (int i = 0; i < fieldN; i++)
			{
				for (int j = 0; j < fieldN; j++)
				{
					map[i, j] = wallN;
				}
			}

			int si = rnd.Next(0, fieldN / 2) * 2 + 1;
			int sj = rnd.Next(0, fieldN / 2) * 2 + 1;

			map[si, sj] = blankN;

			int[] di = { -2, 0, 0, +2 };
			int[] dj = { 0, -2, +2, 0 };

			while (true)
			{

				bool ready = true;
				for (int i = 1; i < fieldN; i += 2)
				{
					for (int j = 1; j < fieldN; j += 2)
					{
						if (map[i, j] != blankN)
						{
							ready = false;
						}
					}
				}

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
						{
							fDeadEnd = false;
						}

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

					if ((ni <= 0) || (nj <= 0) || (ni >= fieldN) || (nj >= fieldN) || (map[ni, nj] == blankN))
					{
						continue;
					}

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
	public int[,] _labyrinthMap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tileMap = GetNode<TileMapLayer>("Node2D/Layer0");
		_tileMap.TileSet.TileSize = new Vector2I(16, 16);
		CallDeferred(nameof(GenerateLabyrinth), 21);
	}

	public void GenerateLabyrinth(int width)
	{
		var generator = new LabyrinthGenerator();
		var map = generator.Generate(width);
		var map2 = generator.Generate(width+2);
		for (int i = 0; i < width - 1; i++)
		{
			for (int j = 0; j < width - 1; j++)
			{
				map[i, j] = map[i, j] * map2[i, j];
			}
		}
		_labyrinthMap = map;

		_tileMap.Clear();

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < width; y++)
			{
				if (map[x, y] == 1) // Wall
				{
					_tileMap.SetCell(new Vector2I(x, y), 0, new Vector2I(3, 3));
				}
				else // Floor
				{
					_tileMap.SetCell(new Vector2I(x, y), 1, Vector2I.Zero);
				}
			}
		}

		// Enable collision on TileMap
		_tileMap.CollisionEnabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
