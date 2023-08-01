using Godot;
using System;

public partial class CheckBoard : Sprite2D
{
	private PackedScene Piece;
	private PackedScene Highlight;
	private PieceMovements pieceMovements;
	public Piece[] board { get; set; }
	public OpponentAI opponent { get; set; }
	public Node2D[] highlight;
	public bool isWhitesTurn { get; set; }
	public bool[] canCastle = { false, false, false, false };
	public int turn;
	public int halfTurn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Piece = ResourceLoader.Load<PackedScene>("res://Scenes/piece.tscn");
		Highlight = ResourceLoader.Load<PackedScene>("res://Scenes/PossibleMoveHighlight.tscn");
		pieceMovements = GetNode<PieceMovements>("PieceMovements");
		opponent = GetNode<OpponentAI>("OpponentAI");

		//Sets up the board. empty spaces are nulls
		board = new Piece[64];
		highlight = new Node2D[64];
		for (int i = 0; i < 64; i++)
		{
			board[i] = null;
			highlight[i] = GenerateHighlight(i);
		}
	}

	private Node2D GenerateHighlight(int index)
	{
		Node2D myHighlight = (Node2D)Highlight.Instantiate();
		AddChild(myHighlight);
		myHighlight.Visible = false;

		int xPos = (index % 8 * 98) + 57;
		int yPos = (((int)Math.Floor((double)index / 8)) * 98) + 55;

		myHighlight.Position = new Vector2(xPos, yPos);

		return myHighlight;
	}

	//This method is used to create pieces
	public Piece GeneratePiece(char type, int index)
	{
		Piece myPiece = (Piece)Piece.Instantiate();
		myPiece.frame = 0;

		//Checks if the piece is white or black
		if (Char.IsUpper(type))
		{
			myPiece.isWhite = true;
		}
		else
		{
			myPiece.isWhite = false;
			myPiece.frame += 6;
		}
		type = Char.ToLower(type);

		//Assigns instance variable with certain data depending on the type of piece
		switch (type)
		{
			case 'p':
				myPiece.name = "pawn";
				myPiece.frame += 5;
				break;

			case 'r':
				myPiece.name = "rook";
				myPiece.frame += 4;
				break;

			case 'n':
				myPiece.name = "night";
				myPiece.frame += 3;
				break;

			case 'b':
				myPiece.name = "bishop";
				myPiece.frame += 2;
				break;

			case 'q':
				myPiece.name = "queen";
				myPiece.frame += 1;
				break;

			case 'k':
				myPiece.name = "king";
				break;
		}
		myPiece.index = index;
		AddChild(myPiece);
		myPiece.InitializePiece();

		return myPiece;
	}

	public void MovePiece(int index1, int index2)
	{
		GD.Print("Choices Made: " + index1 + ", " + index2);
		if (board[index2] == null)
		{
			board[index2] = board[index1];
			board[index2].index = index2;
			board[index2].InitializePiece();
			board[index1] = null;
			if (board[index2].isWhite == true)
			{
				OpponentsTurn();
			}
		}
		else if (board[index1].isWhite != board[index2].isWhite)
		{
			RemoveChild(board[index2]);
			board[index2] = board[index1];
			board[index2].index = index2;
			board[index2].InitializePiece();
			board[index1] = null;
			if (board[index2].isWhite == true)
			{
				OpponentsTurn();
			}
		}
		else
		{
			GD.Print("Invalid Move!");
		}

		if (board[index2] != null)
		{
			if (board[index2].name == "pawn" && index2 <= 7)
			{
				//FIXME: Allow player to choose which piece they would like to promote to with a small popup menu
				//For now, it promotes to queen
				board[index2].name = "queen";
				board[index2].frame -= 4;
				board[index2].InitializePiece();
			}
		}
	}

	//Checks the possible moves of a piece on a given coordinate
	public string CheckMoves(int index)
	{
		string vals = "";
		if (board[index] != null)
		{
			if (board[index].name == "pawn")
			{
				vals = pieceMovements.GetPawnMoves(board, index, vals);
			}
			else if (board[index].name == "rook")
			{
				vals = pieceMovements.GetRookMoves(board, index, vals);
			}
			else if (board[index].name == "night")
			{
				vals = pieceMovements.GetKnightMoves(board, index, vals);
			}
			else if (board[index].name == "bishop")
			{
				vals = pieceMovements.GetBishopMoves(board, index, vals);
			}
			else if (board[index].name == "queen")
			{
				vals = pieceMovements.GetQueenMoves(board, index, vals);
			}
			else if (board[index].name == "king")
			{
				vals = pieceMovements.GetKingMoves(board, index, vals);
				if (vals.Length > 0)
				{
					vals = RemovePuttingKingInCheck(board[index].isWhite, vals + " ");
				}
			}
		}

		return vals;
	}

	public bool IsKingInCheck(bool isWhiteTeam, string kingPos)
	{
		string opponentMoves = "";

		for (int j = 0; j < 64; j++)
		{
			if (board[j] != null)
			{
				if (board[j].isWhite != isWhiteTeam && !board[j].name.Equals("king"))
				{
					opponentMoves += CheckMoves(j);
					if (opponentMoves.Length > 0)
					{
						opponentMoves = " " + opponentMoves + " ";
						if (opponentMoves.Contains(kingPos))
						{
							return true;
						}
						else
						{
							opponentMoves = "";
						}
					}
				}
			}
		}
		return false;
	}

	private string RemovePuttingKingInCheck(bool isWhiteTeam, string vals)
	{
		string finalMoves = "";
		string possibleKingMove = "";
		for (int i = 0; i < vals.Length; i++)
		{
			if (vals[i] == ' ')
			{
				if (!IsKingInCheck(isWhiteTeam, " " + possibleKingMove + " "))
				{
					finalMoves += possibleKingMove + " ";
				}
				possibleKingMove = "";
			}
			else
			{
				possibleKingMove += vals[i];
			}
		}

		if (finalMoves.Length > 0)
		{
			finalMoves = finalMoves.Substring(0, finalMoves.Length - 1);
		}

		return finalMoves;
	}

	public string GetKingPosition(bool isWhiteTeam)
	{
		string kingPos = "";
		for (int i = 0; i < 64; i++)
		{
			if (board[i] != null)
			{
				if (board[i].name.Equals("king") && board[i].isWhite == isWhiteTeam)
				{
					kingPos = " " + i.ToString() + " ";
					break;
				}
			}
		}
		return kingPos;
	}

	private void OpponentsTurn()
	{
		string moves = "";
		moves = CheckMoves(opponent.GetRandomPieceIndex());

		while (moves == "")
		{
			opponent.GetNextIndex();
			if (opponent.piecePosition == -1)
			{
				//Opponent has no viable moves
				break;
			}
			else
			{
				moves = CheckMoves(opponent.piecePosition);
			}
		}

		MovePiece(opponent.piecePosition, opponent.GetRandomMove(moves));
	}

	public bool isCheck(int index1, int index2)
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (board[index2].isWhite != board[index1].isWhite)
				{
				}
			}
		}

		return false;
	}

	//Loops through the board to generate a Fen-style string of the data
	public string CreateFenString()
	{
		string fenString = "";
		int emptySpace = 0;

		for (int i = 0; i < 64; i++)
		{
			if (i % 8 == 0 && i != 0)
			{
				if (emptySpace > 0)
				{
					emptySpace += 1;
					fenString += emptySpace.ToString();
					emptySpace = 0;
				}
				fenString += "/";
			}
			else if (board[i] != null)
			{
				if (emptySpace > 0)
				{
					fenString += emptySpace.ToString();
					emptySpace = 0;
				}
				if (board[i].isWhite == true)
				{
					fenString += Char.ToUpper(board[i].name[0]);
				}
				else
				{
					fenString += board[i].name[0];
				}
			}
			else
			{
				emptySpace += 1;
			}
		}

		fenString += " ";
		if (isWhitesTurn)
		{
			fenString += "w ";
		}
		else
		{
			fenString += "b ";
		}

		if (canCastle[0])
		{
			fenString += "K";
		}
		if (canCastle[1])
		{
			fenString += "Q";
		}
		if (canCastle[2])
		{
			fenString += "k";
		}
		if (canCastle[3])
		{
			fenString += "q";
		}

		//FIXME If there is EnPassant on the board
		//fenString += thatSpace;
		fenString += " - ";

		fenString += turn + " " + halfTurn;

		return fenString;
	}
}