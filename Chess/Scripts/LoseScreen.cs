using Godot;

public partial class LoseScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_menu_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
	}

	public void _on_play_again_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/chessBoard.tscn");
	}

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}