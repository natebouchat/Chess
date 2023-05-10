using Godot;
using System;

public partial class Piece : Node2D
{
	public bool isWhite{get;set;}
	public string name{get;set;}
	public int frame{get;set;}
	public int index{get;set;}
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
		
		int xPos = ((index % 8) * 98) - 397;
		int yPos = (((int)Math.Floor((double)index / 8)) * 98) - 400;
		
		this.Position = new Vector2(xPos, yPos);

	}
	
	public void RemovePiece(){
		anim.Stop();
	}

	public string toString(){
		string info = "isWhite: " + isWhite + "    " + name + "  " + index; 
		return info;
	}


}
