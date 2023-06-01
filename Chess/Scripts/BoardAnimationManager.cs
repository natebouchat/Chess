using Godot;
using System;

public partial class BoardAnimationManager : AnimationPlayer
{
	Label checkStatusText;

	public override void _Ready()
	{
		checkStatusText = GetNode<Label>("Check");
		checkStatusText.Modulate = new Color(1,1,1,0);
	}

	public void PlayInCheckAnimation(bool isWhiteTeam) {
		if(isWhiteTeam) {
			checkStatusText.Text = "Check!\nWhite Team";
			checkStatusText.AddThemeColorOverride("font_color", new Color(1,1,1));
			checkStatusText.AddThemeColorOverride("font_outline_color", new Color(0,0,0));
		}
		else {
			checkStatusText.Text = "Check!\nBlack Team";
			checkStatusText.AddThemeColorOverride("font_color", new Color(0,0,0));
			checkStatusText.AddThemeColorOverride("font_outline_color", new Color(1,1,1));
		}
		
		this.Play("KingInCheck");
	}

	public async void PlayCheckmateTransition(bool isWhiteTeam) {
		GetNode<Sprite2D>("Background").Visible = true;
		this.Play("CheckmateTransition");
		await ToSignal(this, "animation_finished");
		if(isWhiteTeam) {
			GetTree().ChangeSceneToFile("res://Scenes/LoseScreen.tscn");
		}
		else {
			GetTree().ChangeSceneToFile("res://Scenes/WinScreen.tscn");
		}
	}
}
