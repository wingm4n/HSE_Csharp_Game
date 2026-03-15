using Godot;
using System;

public partial class Board : Node2D
{
	public const int Size = 4;
	public Tile[,] Tiles = new Tile[Size, Size];
	public Vector2I EmptyPos = new Vector2I(Size-1, Size-1);

	public override void _Ready()
	{
		InitializeBoard();
	}

	public void InitializeBoard()
	{
		int num = 1;
		for(int y=0; y<Size; y++)
		{
			for(int x=0; x<Size; x++)
			{
				Tile tile = new Tile();
				if(x==Size-1 && y==Size-1)
				{
					tile.IsEmpty = true;
					tile.Number = 0;
					EmptyPos = new Vector2I(x,y);
				}
				else tile.Number = num++;

				tile.GridPos = new Vector2I(x,y);
				tile.UpdatePosition(tile.GridPos);

				Tiles[x,y] = tile;
				AddChild(tile);
			}
		}
	}

	public void MoveTile(Vector2I dir)
	{
		Vector2I target = EmptyPos + dir;
		if(target.X<0 || target.X>=Size || target.Y<0 || target.Y>=Size) 
		{
			return;
		}
		Tile t = Tiles[target.X,target.Y];

		Tiles[EmptyPos.X,EmptyPos.Y] = t;
		t.UpdatePosition(EmptyPos);
		Tiles[target.X,target.Y] = null;

		EmptyPos = target;
	}
}
