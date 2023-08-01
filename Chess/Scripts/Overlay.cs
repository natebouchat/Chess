using Godot;

public partial class Overlay : ColorRect
{
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			GetTree().Paused = !GetTree().Paused;
			Visible = !Visible;
		}
	}
}