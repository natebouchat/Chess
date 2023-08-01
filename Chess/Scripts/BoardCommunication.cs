using Godot;
using System;

public partial class BoardCommunication : Node2D
{
	private CheckBoard checkBoard;
	private BoardAnimationManager animationManager;
	private PackedScene BoardButtonScene;
	private BoardButton[] boardButtons;
	private string fenPosition;
	private int lastClickedPosition = -1;

	public override void _Ready()
	{
		checkBoard = GetNode<CheckBoard>("Board");
		animationManager = GetNode<BoardAnimationManager>("AnimationPlayer");
		BoardButtonScene = ResourceLoader.Load<PackedScene>("res://Scenes/BoardButton.tscn");
		boardButtons = new BoardButton[64];
		for (int i = 0; i < 64; i++)
		{
			boardButtons[i] = GenerateBoardButton(i);
		}

		fenPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
		GD.Print(fenPosition);
		LoadBoard(fenPosition);
	}

	private void LoadBoard(string fenData)
	{
		string data = fenData.Substring(0, fenData.IndexOf(' '));
		int i;

		for (i = 0; i < 64; i++)
		{
			if (data[0] == '/')
			{
				i -= 1;
			}
			else if (char.IsLetter(data[0]))
			{
				checkBoard.board[i] = checkBoard.GeneratePiece(data[0], i);
			}
			else if (char.IsDigit(data[0]))
			{
				string temp = data.Substring(0, 1);
				int value = Int32.Parse(temp);

				for (int z = 0; z < value; z++)
				{
					if (checkBoard.board[i + z] != null)
					{
						RemoveChild(checkBoard.board[i + z]);
						checkBoard.board[i + z] = null;
					}
				}
				i += value - 1;
			}
			data = data.Substring(1);
		}

		data = fenData.Substring(fenData.IndexOf(' ') + 1);
		if (data[0] == 'w')
		{
			checkBoard.isWhitesTurn = true;
		}
		else
		{
			checkBoard.isWhitesTurn = false;
		}
		data = data.Substring(2);

		while (data[0] != ' ')
		{
			switch (data[0])
			{
				case 'K':
					checkBoard.canCastle[0] = true;
					break;

				case 'Q':
					checkBoard.canCastle[1] = true;
					break;

				case 'k':
					checkBoard.canCastle[2] = true;
					break;

				case 'q':
					checkBoard.canCastle[3] = true;
					break;
			}
			data = data.Substring(1);
		}
		data = data.Substring(1);

		if (data[0] != '-')
		{
			//FIXME Add an En Passant move to the pawns nearby.
		}
		else
		{
			data = data.Substring(2);
		}
		checkBoard.turn = Int32.Parse(data.Substring(0, data.IndexOf(' ')));
		checkBoard.halfTurn = Int32.Parse(data.Substring(data.IndexOf(' ') + 1));
	}

	public void ButtonWasClicked(int position)
	{
		if (lastClickedPosition >= 0 && checkBoard.highlight[position].Visible == true)
		{
			GD.Print("From " + lastClickedPosition + " to " + position);
			checkBoard.MovePiece(lastClickedPosition, position);
			lastClickedPosition = -1;
			ClearHighlights();
			CheckWinConditions();
		}

		if (checkBoard.board[position] != null)
		{
			GD.Print("Position: " + checkBoard.board[position].index + ", Piece: " + checkBoard.board[position].name);
			lastClickedPosition = position;
			ClearHighlights();
			if (checkBoard.board[position].isWhite == true)
			{
				DisplayHighlights(checkBoard.CheckMoves(position));
			}
		}
		else
		{
			GD.Print("Position: " + position + ", Piece: NULL");
			lastClickedPosition = -1;
			ClearHighlights();
		}
	}

	private BoardButton GenerateBoardButton(int boardPos)
	{
		BoardButton button = (BoardButton)BoardButtonScene.Instantiate();
		AddChild(button);
		button.InitializeBoardButton(boardPos);
		return button;
	}

	public int[] DisplayHighlights(string vals)
	{
		string[] temp;
		int[] possibleMoves;
		if (vals.Length > 0)
		{
			temp = vals.Split(' ');
			possibleMoves = new int[temp.Length];
			for (int i = 0; i < temp.Length; i++)
			{
				Console.WriteLine(i + ": " + temp[i]);
				try
				{
					possibleMoves[i] = Int32.Parse(temp[i]);
				}
				catch (Exception e)
				{
					GD.Print("ERROR in BoardCommunication (DisplayHighlights()), placing null in array instead");
					GD.Print(e);
				}
				checkBoard.highlight[possibleMoves[i]].Visible = true;
			}
			return possibleMoves;
		}
		else
		{
			GD.Print("No Vals to Highlight Moves (122 of InitializeBoard)");
			return null;
		}
	}

	public void ClearHighlights()
	{
		for (int i = 0; i < 64; i++)
		{
			checkBoard.highlight[i].Visible = false;
		}
	}

	public void CheckWinConditions()
	{
		//Check both team king status
		string kingPos = checkBoard.GetKingPosition(true);
		string kingMoves;
		if (checkBoard.IsKingInCheck(true, kingPos))
		{
			kingPos = kingPos.Substring(1, kingPos.Length - 2);
			kingMoves = checkBoard.CheckMoves(Int32.Parse(kingPos));
			if (kingMoves.Length == 0)
			{
				animationManager.PlayCheckmateTransition(true);
				//GD.PrintRich("[b][color=red]!!! CHECKMATE: White Team Has Lost!!![/color][/b]");
			}
			else
			{
				animationManager.PlayInCheckAnimation(true);
				//GD.PrintRich("[b]!!! CHECK: White Team !!![/b]");
			}
		}

		kingPos = checkBoard.GetKingPosition(false);
		if (checkBoard.IsKingInCheck(false, kingPos))
		{
			kingPos = kingPos.Substring(1, kingPos.Length - 2);
			kingMoves = checkBoard.CheckMoves(Int32.Parse(kingPos));
			if (kingMoves.Length == 0)
			{
				animationManager.PlayCheckmateTransition(false);
				//GD.PrintRich("[b][color=green]!!! CHECKMATE: White Team Has Won !!![/color][/b]");
			}
			else
			{
				animationManager.PlayInCheckAnimation(false);
				//GD.PrintRich("[b]!!! CHECK: Black Team !!![/b]");
			}
		}
	}
}