using Godot;
using System;
using System.Collections.Generic;

public partial class OpponentAI : Node
{
	public int piecePosition {get; set;}
	private int arrayIndex;
	public List<int> opponentPiecePositions;
	private Random random;
	private int checkCount;
	int[] possibleMoves;
	private int movesArraySize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		opponentPiecePositions = new List<int>();
		for(int i = 0; i < 16; i++) {
			opponentPiecePositions.Add(i);
		}
		random  = new Random();
		checkCount = 0;
	}

	public int GetRandomPieceIndex() {
		arrayIndex = random.Next(opponentPiecePositions.Count);
		checkCount = 1;
		piecePosition = opponentPiecePositions[arrayIndex];
		return opponentPiecePositions[arrayIndex];
	}

	public int GetNextIndex() {
		arrayIndex++;
		
		if(checkCount == opponentPiecePositions.Count) {
			// Opponent does not have any viable moves.
			arrayIndex = -1;
			return -1;
		}
		else if (arrayIndex >= opponentPiecePositions.Count) {
			arrayIndex = 0;
		}

		piecePosition = opponentPiecePositions[arrayIndex];
		return opponentPiecePositions[arrayIndex];
	}

	public int GetRandomMove(string moves) {
		int randomMove;
		randomMove = ParseMovesString(moves)[random.Next(movesArraySize)];
		opponentPiecePositions[arrayIndex] = randomMove;
		return randomMove;
	}

	private int[] ParseMovesString(string moves) {
		string[] temp;
		if(moves.Length > 0){
			temp = moves.Split(' ');
			possibleMoves = new int[temp.Length];
			movesArraySize = temp.Length;
			for(int i = 0; i < temp.Length; i++){
				possibleMoves[i] = Int32.Parse(temp[i]);
			}
		}
		return possibleMoves;
	}

}
