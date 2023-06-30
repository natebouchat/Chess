using Godot;
using System;

public partial class menu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_play_pressed()
	{
	GetTree().ChangeSceneToFile("res://Scenes/chessBoard.tscn");
	}
	
	private void _on_multiplayer_pressed()
	{
	GetTree().ChangeSceneToFile("res://Scenes/chessBoard.tscn");
	}
	
	private void _on_quit_pressed()
	{
	GetTree().Quit();
	}
}
