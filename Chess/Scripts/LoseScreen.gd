extends Node2D

func _on_quit_pressed():
	get_tree().quit()


func _on_play_again_pressed():
	get_tree().change_scene_to_file("res://Scenes/chessBoard.tscn")



func _on_menu_pressed():
	get_tree().change_scene_to_file("res://Scenes/menu.tscn")
