using Godot;
using System;

public partial class OpponentAI : Node
{
	public int piecePosition {get; set;}
	private int arrayIndex;
	public int[] opponentPiecePositions = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};
	private Random random = new Random();
	private int checkCount;
	int[] possibleMoves;
	private int movesArraySize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		checkCount = 0;
	}

	public int GetRandomPieceIndex() {
		arrayIndex = random.Next(opponentPiecePositions.Length);
		checkCount = 1;
		piecePosition = opponentPiecePositions[arrayIndex];
		return opponentPiecePositions[arrayIndex];
	}

	public int GetNextIndex() {
		arrayIndex++;
		
		if(checkCount == opponentPiecePositions.Length) {
			// Opponent does not have any viable moves.
			arrayIndex = -1;
			return -1;
		}
		else if (arrayIndex >= opponentPiecePositions.Length) {
			arrayIndex = 0;
		}

		piecePosition = opponentPiecePositions[arrayIndex];
		//return opponentPiecePositions[arrayIndex];
		return 15;
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
