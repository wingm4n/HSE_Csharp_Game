using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Node2D
{
	public const int Size = 3;
	public Tile[,] Tiles = new Tile[Size, Size];
	public Vector2I EmptyPos = new Vector2I(Size - 1, Size - 1);

	public event Action OnGameWon;

	private Random _rng = new Random();

	private static readonly Vector2I[] Directions = {
		new Vector2I(0,  1),
		new Vector2I(0, -1),
		new Vector2I(1,  0),
		new Vector2I(-1, 0)
	};

	public override void _Ready()
	{
		InitializeBoard();
		CallDeferred(nameof(CenterBoard));
	}

	private void CenterBoard()
	{
		var viewport = GetViewportRect();
		int boardPixels = Size * Tile.TileStep - Tile.TileGap;
		Position = new Vector2(
			(viewport.Size.X - boardPixels) / 2f,
			(viewport.Size.Y - boardPixels) / 2f
		);
	}

	public void InitializeBoard()
	{
		foreach (Node child in GetChildren())
			child.QueueFree();

		int[] state = GenerateSolvableState();

		for (int y = 0; y < Size; y++)
		{
			for (int x = 0; x < Size; x++)
			{
				int idx = y * Size + x;
				int num = state[idx];

				Tile tile = new Tile();
				tile.Number  = num;
				tile.IsEmpty = (num == 0);

				if (tile.IsEmpty)
					EmptyPos = new Vector2I(x, y);

				tile.GridPos = new Vector2I(x, y);
				Tiles[x, y] = tile;
				AddChild(tile);
				tile.UpdatePosition(tile.GridPos);
			}
		}
	}

	private int[] GenerateSolvableState()
	{
		int[] state = new int[Size * Size];
		for (int i = 0; i < Size * Size - 1; i++)
			state[i] = i + 1;
		state[Size * Size - 1] = 0;

		int emptyX = Size - 1;
		int emptyY = Size - 1;
		int lastOpposite = -1;

		for (int step = 0; step < 200; step++)
		{
			List<int> valid = new List<int>();
			for (int d = 0; d < Directions.Length; d++)
			{
				if (d == lastOpposite) continue;
				int nx = emptyX + Directions[d].X;
				int ny = emptyY + Directions[d].Y;
				if (nx >= 0 && nx < Size && ny >= 0 && ny < Size)
					valid.Add(d);
			}

			int chosen = valid[_rng.Next(valid.Count)];
			int tx = emptyX + Directions[chosen].X;
			int ty = emptyY + Directions[chosen].Y;

			int emptyIdx  = emptyY * Size + emptyX;
			int targetIdx = ty    * Size + tx;
			state[emptyIdx]  = state[targetIdx];
			state[targetIdx] = 0;

			emptyX = tx;
			emptyY = ty;
			lastOpposite = chosen % 2 == 0 ? chosen + 1 : chosen - 1;
		}

		return state;
	}

	public void MoveTile(Vector2I dir)
	{
		Vector2I target = EmptyPos + dir;
		if (target.X < 0 || target.X >= Size || target.Y < 0 || target.Y >= Size)
			return;

		Tile t = Tiles[target.X, target.Y];
		Tiles[EmptyPos.X, EmptyPos.Y] = t;
		t.UpdatePosition(EmptyPos);
		Tiles[target.X, target.Y] = null;
		EmptyPos = target;

		if (CheckWin())
			OnGameWon?.Invoke();
	}

	private bool CheckWin()
	{
		int expected = 1;
		for (int y = 0; y < Size; y++)
		{
			for (int x = 0; x < Size; x++)
			{
				if (x == Size - 1 && y == Size - 1) break;
				if (Tiles[x, y] == null || Tiles[x, y].Number != expected)
					return false;
				expected++;
			}
		}
		return true;
	}
}
