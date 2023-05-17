using Godot;
using System;

public partial class PieceMovements : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public string GetPawnMoves(Piece[] board, int index, string vals) {
		if(board[index].isWhite){
			if(index - 8 >= 0 && board[index - 8] == null){
				vals += (index - 8) + " ";
				if(index - 18 > 0 && board[index - 16] == null && ((int)Math.Floor((double)index / 8)) == 6){
					vals += (index - 16) + " ";
				}
			}
			if(index - 7 >= 0 && board[index - 7] != null && board[index - 7].isWhite != board[index].isWhite){
				vals += (index - 7) + " ";
			}
			if(index - 9 >= 0 && board[index - 9] != null && board[index - 9].isWhite != board[index].isWhite){
				vals += (index - 9) + " ";
			}
		}
		else{
			if(index + 8 < 64 && board[index + 8] == null){
				vals += (index + 8) + " ";
				if(index + 16 < 64 && board[index + 16] == null && ((int)Math.Floor((double)index / 8)) == 1){
					vals += (index + 16) + " ";
				}
			}
			if(index + 9 < 64 && board[index + 9] != null && board[index + 9].isWhite != board[index].isWhite){
				vals += (index + 9) + " ";
			}
			if(index + 7 < 64 && board[index + 7] != null && board[index + 7].isWhite != board[index].isWhite){
				vals += (index + 7) + " ";
			}
		}
		if(vals.Length > 0){
			vals = vals.Substring(0, vals.Length - 1);
		}
		return vals;
	}

	public string GetRookMoves(Piece[] board, int index, string vals) {
		for(int i = index - 8; i >= 0; i -= 8){
			if(board[i] != null){
				if(board[i].isWhite != board[index].isWhite){
					vals += i + " ";
				}
				break;
			}
			vals += i + " ";
		}
		for(int i = index + 8; i < 64; i += 8){
			if(board[i] != null){
				if(board[i].isWhite != board[index].isWhite){
					vals += i + " ";
				}
				break;
			}
			vals += i + " ";
		}
		if(index % 8 != 0){
			for(int i = index - 1; i >= 0; i--){
				if(board[i] != null){
					if(board[i].isWhite != board[index].isWhite){
						vals += i + " ";
					}
					break;
				}
				vals += i + " ";
				if(i % 8 == 0){
					break;
				}
			}
		}
		if(index % 8 != 7){
			for(int i = index + 1; i < 64; i++){
				if(board[i] != null){
					if(board[i].isWhite != board[index].isWhite){
						vals += i + " ";
					}
					break;
				}
				vals += i + " ";
				if(i % 8 == 7){
					break;
				}
			}
		}
		if(vals.Length > 0){
			vals = vals.Substring(0, vals.Length - 1);
		}
		return vals;
	}

	public string GetKnightMoves(Piece[] board, int index, string vals) {
		if(index - 17 >= 0 && (board[index - 17] == null || board[index - 17].isWhite != board[index].isWhite)){
			vals += (index - 17) + " ";
		}
		if(index - 15 >= 0 && (board[index - 15] == null || board[index - 15].isWhite != board[index].isWhite)){
			vals += (index - 15) + " ";
		}
		if(index - 10 >= 0 && (board[index - 10] == null || board[index - 10].isWhite != board[index].isWhite)){
			vals += (index - 10) + " ";
		}
		if(index - 6 >= 0 && (board[index - 6] == null || board[index - 6].isWhite != board[index].isWhite)){
			vals += (index - 6) + " ";
		}
		if(index + 17 < 64 && (board[index + 17] == null || board[index + 17].isWhite != board[index].isWhite)){
			vals += (index + 17) + " ";
		}
		if(index + 15 < 64 && (board[index + 15] == null || board[index + 15].isWhite != board[index].isWhite)){
			vals += (index + 15) + " ";
		}
		if(index + 17 < 64 && (board[index + 10] == null || board[index + 10].isWhite != board[index].isWhite)){
			vals += (index + 10) + " ";
		}
		if(index + 6 < 64 && (board[index + 6] == null || board[index + 6].isWhite != board[index].isWhite)){
			vals += (index + 6) + " ";
		}
		if(vals.Length > 0){
			vals = vals.Substring(0, vals.Length - 1);
		}
		return vals;
	}

	public string GetBishopMoves(Piece[] board, int index, string vals) {
		if(index % 8 != 7){
			for(int i = index + 9; i < 64; i += 9){
				if(board[i] != null){
					if(board[i].isWhite != board[index].isWhite){
						vals += i + " ";
					}
					break;
				}
				vals += i + " ";
				if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)i / 8)) == 0 || ((int)Math.Floor((double)i / 8)) == 7){
					break;
				}
			}
			for(int i = index - 7; i >= 0; i -= 7){
				if(board[i] != null){
					if(board[i].isWhite != board[index].isWhite){
						vals += i + " ";
					}
					break;
				}
				vals += i + " ";
				if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)i / 8)) == 0 || ((int)Math.Floor((double)i / 8)) == 7){
					break;
				}
			}
		}
		if(index % 8 != 0){
			for(int i = index + 7; i < 64; i += 7){
				if(board[i] != null){
					if(board[i].isWhite != board[index].isWhite){
						vals += i + " ";
					}
					break;
				}
				vals += i + " ";
				if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)i / 8)) == 0 || ((int)Math.Floor((double)i / 8)) == 7){
					break;
				}
			}
			for(int i = index - 9; i >= 0; i -= 9){
				if(board[i] != null){
					if(board[i].isWhite != board[index].isWhite){
						vals += i + " ";
					}
					break;
				}
				vals += i + " ";
				if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)i / 8)) == 0 || ((int)Math.Floor((double)i / 8)) == 7){
					break;
				}
			}
		}
		if(vals.Length > 0){
			vals = vals.Substring(0, vals.Length - 1);
		}
		return vals;
	}	

	public string GetQueenMoves(Piece[] board, int index, string vals) {
		string newVals = "";
		newVals += GetRookMoves(board, index, vals);
		if(newVals.Length > 0){
			newVals += " ";
		}
		newVals += GetBishopMoves(board, index, vals);
		return newVals;
	}	

	public string GetKingMoves(Piece[] board, int index, string vals) {
		if(index - 9 >= 0 && (board[index - 9] == null || board[index - 9].isWhite != board[index].isWhite)){
		vals += (index - 9) + " ";
		}
		if(index - 8 >= 0 && (board[index - 8] == null || board[index - 8].isWhite != board[index].isWhite)){
			vals += (index - 8) + " ";
		}
		if(index - 7 >= 0 && (board[index - 7] == null || board[index - 7].isWhite != board[index].isWhite)){
			vals += (index - 7) + " ";
		}
		if(index - 1 >= 0 && (board[index - 1] == null || board[index - 1].isWhite != board[index].isWhite)){
			vals += (index - 1) + " ";
		}
		if(index + 1 < 64 && (board[index + 1] == null || board[index + 1].isWhite != board[index].isWhite)){
			vals += (index + 1) + " ";
		}
		if(index + 7 < 64 && (board[index + 7] == null || board[index + 7].isWhite != board[index].isWhite)){
			vals += (index + 7) + " ";
		}
		if(index + 8 < 64 && (board[index + 8] == null || board[index + 8].isWhite != board[index].isWhite)){
			vals += (index + 8) + " ";
		}
		if(index + 9 < 64 && (board[index + 9] == null || board[index + 9].isWhite != board[index].isWhite)){
			vals += (index + 9) + " ";
		}
		if(vals.Length > 0){
			vals = vals.Substring(0, vals.Length - 1);
		}
			return vals;
	}
}
