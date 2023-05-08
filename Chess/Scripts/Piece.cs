using Godot;
using System;

public partial class Piece : Node2D
{
	public bool isWhite{get;set;}
	public string name{get;set;}
	public int frame{get;set;}
	public int xCoord{get;set;}
	public int yCoord{get;set;}
	private AnimatedSprite2D anim;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("Sprites");
	}

	public override void _Process(double delta)
	{

	}
	
	public void InitializePiece() {
		anim.Frame = frame;
		
		//Each board square has a side length of 98 pixels currently, and the border is 9 pixels.
		//Coords are the array value, so to offset the piece sprites to the correct position on the board,
		//I will multiply the value by 98 for each square, then add 9.
		int xPos = (xCoord * 98) - 397;
		int yPos = (yCoord * 98) - 400;
		this.Position = new Vector2(xPos, yPos);
	}



}
