using Godot;
using System;

public partial class Piece : Node2D
{
	public bool p1{get;set;}
	public string name{get;set;}
	public int frame{get;set;}
	private AnimatedSprite2D anim;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite2D>("Sprites");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		anim.Frame = frame;
	}
}
