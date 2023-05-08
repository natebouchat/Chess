using Godot;
using System;

public partial class CheckBoard : Sprite2D
{
	private PackedScene Piece;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Piece = ResourceLoader.Load<PackedScene>("res://Scenes/piece.tscn");
		
		Piece pawn = (Piece)Piece.Instantiate();
		pawn.p1 = true;
		pawn.name = "pawn";
		pawn.frame = 5;
		AddChild(pawn);
		
		/*
		//The board
		Piece[,] board = new Piece[8,8];
		
		Piece pawn = getNode<Piece>("Piece");
		pawn.p1 = true;
		pawn.name = "pawn";
		
		Piece rook = getNode<Piece>("Piece");
		rook.p1 = false;
		
		Piece knight = getNode<Piece>("Piece");
		Piece bishop = getNode<Piece>("Piece");
		Piece queen = getNode<Piece>("Piece");
		Piece king = getNode<Piece>("Piece");
		*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
