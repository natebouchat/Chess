using Godot;
using System;

public partial class CheckBoard : Sprite2D
{
	private PackedScene Piece;
	public string fenPosition{get;set;}
	public Piece[,] board;
	public bool isWhitesTurn{get;set;}
	public bool[] canCastle = {false, false, false, false};
	public int turn;
	public int halfTurn;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fenPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
		Piece = ResourceLoader.Load<PackedScene>("res://Scenes/piece.tscn");
		
		//Sets up the board. empty spaces are nulls
		board = new Piece[8,8];
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				board[i, j] = null;
			}
		}
		
		//
		GD.Print(fenPosition);
		LoadBoard(fenPosition);
		GD.Print(CreateFenString() + "\n");
		

		GD.Print(CheckMoves(0, 1));
		GD.Print(CheckMoves(0, 6));
		
		GD.Print(CheckMoves(0, 0));
		MovePiece(0, 0, 4, 4);
		GD.Print(CheckMoves(4, 4));
		MovePiece(4, 4, 0, 0);
		
		GD.Print(CheckMoves(1, 0));
		MovePiece(1, 0, 4, 3);
		GD.Print(CheckMoves(4, 3));
		MovePiece(4, 3, 1, 0);
		
		GD.Print(CheckMoves(2, 0));
		MovePiece(2, 0, 4, 4);
		GD.Print(CheckMoves(4, 4));
		MovePiece(4, 4, 2, 0);
		
		GD.Print(CheckMoves(3, 0));
		MovePiece(3, 0, 4, 4);
		GD.Print(CheckMoves(4, 4));
		MovePiece(4, 4, 3, 0);
		
		GD.Print(CheckMoves(4, 0));
		MovePiece(4, 0, 4, 4);
		GD.Print(CheckMoves(4, 4));
		MovePiece(4, 4, 4, 0);
		
		/*
		fenPosition = "rnbqkbnr/pppppppp/8/8/3P4/8/PPP1PPPP/RNBQKBNR b KQkq - 0 1";
		GD.Print(fenPosition);
		LoadBoard(fenPosition);
		GD.Print(CreateFenString() + "\n");
		
		fenPosition = "pppppppp/pppppppp/pppppppp/pppppppp/pppppppp/pppppppp/ppppppppP/RNBQKBNR b KQkq - 0 1";
		GD.Print(fenPosition);
		LoadBoard(fenPosition);
		GD.Print(CreateFenString() + "\n");
		*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	//Loads a board based on the string given to it
	public void LoadBoard(string fenData){
		string data = fenData.Substring(0, fenData.IndexOf(' '));
		int i = 0;
		int j = 0;
		
		for(i = 0; i < 8; i++){
			j = 0;
			while(data.Length > 0 && data[0] != '/'){
				if(char.IsLetter(data[0]) && j < 8){
					board[i, j] = GeneratePiece(data[0], j, i);
				}else if(char.IsDigit(data[0])){
					
					string temp = data.Substring(0, 1);
					int value = Int32.Parse(temp);
					
					for(int z = 0; z < value; z++){
						if(board[i, j + z] != null){
							board[i, j + z].RemovePiece();
							board[i, j + z] = null;
						}
					}
					j += value - 1;
				}
				data = data.Remove(0, 1);
				j++;
			}
			if(data.Length > 0){
				data = data.Remove(0, 1);
			}
		}
		
		data = fenData.Substring(fenData.IndexOf(' ') + 1);
		if(data[0] == 'w'){
			isWhitesTurn = true;
		}else{
			isWhitesTurn = false;
		}
		data = data.Substring(2);
		
		while(data[0] != ' '){
			switch(data[0]){
				case 'K':
					canCastle[0] = true;
					break;
				case 'Q':
					canCastle[1] = true;
					break;
				case 'k':
					canCastle[2] = true;
					break;
				case 'q':
					canCastle[3] = true;
					break;
			}
			data = data.Substring(1);
		}
		data = data.Substring(1);
		
		if(data[0] != '-'){
			//Add an En Passant move to the pawns nearby.
		}else{
			data = data.Substring(2);
		}
		
		turn = Int32.Parse(data.Substring(0, data.IndexOf(' ')));
		
		halfTurn = Int32.Parse(data.Substring(data.IndexOf(' ') + 1));
	}
	
	
	//This method is used to create pieces
	public Piece GeneratePiece(char type, int xCoord, int yCoord){
		Piece myPiece = (Piece)Piece.Instantiate();
		myPiece.frame = 0;
		
		//Checks if the piece is white or black
		if(Char.IsUpper(type)){
			myPiece.isWhite = true;
		}else{
			myPiece.isWhite = false;
			myPiece.frame += 6;
		}
		type = Char.ToLower(type);
		
		//Assigns instance variabel with certain data depending on the type of piece
		switch(type){
			case 'p':
				myPiece.name = "pawn";
				myPiece.frame += 5;
				break;
			case 'r':
				myPiece.name = "rook";
				myPiece.frame += 4;
				break;
			case 'n':
				myPiece.name = "knight";
				myPiece.frame += 3;
				break;
			case 'b':
				myPiece.name = "bishop";
				myPiece.frame += 2 ;
				break;
			case 'q':
				myPiece.name = "queen";
				myPiece.frame += 1 ;
				break;
			case 'k':
				myPiece.name = "king";
				break;
		}

		myPiece.xCoord = xCoord;
		myPiece.yCoord = yCoord;

		AddChild(myPiece);
		myPiece.InitializePiece();

		return myPiece;
	}
	
	//moves a piece from one position to another
	public void MovePiece(int x1, int y1, int x2, int y2){
		if(board[y2, x2] == null){
			board[y2, x2] = board[y1, x1];
			board[y2, x2].xCoord = x2;
			board[y2, x2].yCoord = y2;
			board[y2, x2].InitializePiece();
			board[y1, x1] = null;
		}else if(board[y1, x1].isWhite != board[y2, x2].isWhite){
			//Remove the piece at x2 y2
			board[y2, x2] = board[y1, x1];
			board[y2, x2].xCoord = x2;
			board[y2, x2].yCoord = y2;
			board[y2, x2].InitializePiece();
			board[y1, x1] = null;
		}else{
			GD.Print("Invalid Move!");
		}
	}
	
	
	//Checks the possible moves of a piece on a given coordinate
	public string CheckMoves(int x, int y){
		int numMoves = 0;
		string vals = "";
		
		if(board[y, x].name == "pawn"){
			GD.Print(board[y, x].toString());
			if(board[y, x].isWhite){
				if(y - 1 > 0 && board[y - 1, x] == null){
					vals += x + " " + (y - 1) + " ";
					if(y - 2 > 0 && board[y - 2, x] == null && y == 6){
						vals += x + " " + (y - 2) + " ";
					}
				}
				if(y - 1 > 0 && x + 1 < 8 && board[y - 1, x + 1] != null && board[y - 1, x + 1].isWhite != board[y, x].isWhite){
					vals += (x + 1) + " " + (y - 1) + " ";
				}
				if(y - 1 > 0 && x - 1 > 0 && board[y - 1, x - 1] != null && board[y - 1, x - 1].isWhite != board[y, x].isWhite){
					vals += (x - 1) + " " + (y - 1) + " ";
				}
			}else{
				if(y + 1 < 8 && board[y + 1, x] == null){
					vals += x + " " + (y + 1) + " ";

					if(y + 2 < 8 && board[y + 2, x] == null && y == 1){
						vals += x + " " + (y + 2) + " ";
					}
				}
				if(y + 1 < 8 && x + 1 < 8 && board[y + 1, x + 1] != null && board[y + 1, x + 1].isWhite != board[y, x].isWhite){
					vals += (x + 1) + " " + (y + 1) + " ";
				}
				if(y + 1 < 8 && x - 1 > 0 && board[y + 1, x - 1] != null && board[y + 1, x - 1].isWhite != board[y, x].isWhite){
					vals += (x - 1) + " " + (y + 1) + " ";
				}
			}
		}
		
		if(board[y, x].name == "rook"){
			GD.Print(board[y, x].toString());
			int i = 0;
			
			while(y + i >= 0){
				if(board[y + i, x] == null){
					vals += x + " " + (y + i) + " ";
 				}else if(board[y + i, x].isWhite != board[y, x].isWhite){
					vals += x + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x].isWhite == board[y, x].isWhite){
					break;
				}
				i--;
			}
			i = 0;
			
			while(y + i < 8){
				if(board[y + i, x] == null){
					vals += x + " " + (y + i) + " ";
 				}else if(board[y + i, x].isWhite != board[y, x].isWhite){
					vals += x + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(x + i >= 0){
				if(board[y, x + i] == null){
					vals += (x + i) + " " + y + " ";
 				}else if(board[y, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + y + " ";
					break;
				}else if(i != 0 && board[y, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i--;
			}
			i = 0;
			
			while(x + i < 8){
				if(board[y, x + i] == null){
					vals += (x + i) + " " + y + " ";
 				}else if(board[y, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + y + " ";
					break;
				}else if(i != 0 && board[y, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
		}
		
		if(board[y, x].name == "knight"){
			GD.Print(board[y, x].toString());
			int x2;
			int y2;
			
			for(x2 = 2; x2 > -3; x2--){
				if(x2 == 0){
					continue;
				}
				if(x2 % 2 == 0){
					y2 = 1;
				}else{
					y2 = 2;
				}
				
				if(x + x2 < 8 && x + x2 >= 0){
					if(y + y2 < 8){
						if(board[y + y2, x + x2] == null || board[y + y2, x + x2].isWhite != board[y, x].isWhite){
							vals += (x + x2) + " " + (y + y2) + " ";
						}
					}
					if(y - y2 >= 0){
						if(board[y - y2, x + x2] == null || board[y - y2, x + x2].isWhite != board[y, x].isWhite){
							vals += (x + x2) + " " + (y - y2) + " ";
						}
					}
				}
			}
		}
		
		if(board[y, x].name == "bishop"){
			GD.Print(board[y, x].toString());
			int i = 0;
			
			while(y + i < 8 && x + i < 8){
				if(board[y + i, x + i] == null){
					vals += (x + i) + " " + (y + i) + " ";
 				}else if(board[y + i, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(y + i < 8 && x - i >= 0){
				if(board[y + i, x - i] == null){
					vals += (x - i) + " " + (y + i) + " ";
 				}else if(board[y + i, x - i].isWhite != board[y, x].isWhite){
					vals += (x - i) + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x - i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(y - i >= 0 && x + i < 8){
				if(board[y - i, x + i] == null){
					vals += (x + i) + " " + (y - i) + " ";
 				}else if(board[y - i, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + (y - i) + " ";
					break;
				}else if(i != 0 && board[y - i, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(y - i >= 0 && x - i < 8){
				if(board[y - i, x - i] == null){
					vals += (x - i) + " " + (y - i) + " ";
 				}else if(board[y - i, x - i].isWhite != board[y, x].isWhite){
					vals += (x - i) + " " + (y - i) + " ";
					break;
				}else if(i != 0 && board[y - i, x - i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
		}
		
		if(board[y, x].name == "queen"){
		GD.Print(board[y, x].toString());
		int i = 0;
		
			//Diagonal Checks
			while(y + i < 8 && x + i < 8){
				if(board[y + i, x + i] == null){
					vals += (x + i) + " " + (y + i) + " ";
					}else if(board[y + i, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(y + i < 8 && x - i >= 0){
				if(board[y + i, x - i] == null){
					vals += (x - i) + " " + (y + i) + " ";
					}else if(board[y + i, x - i].isWhite != board[y, x].isWhite){
					vals += (x - i) + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x - i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(y - i >= 0 && x + i < 8){
				if(board[y - i, x + i] == null){
					vals += (x + i) + " " + (y - i) + " ";
					}else if(board[y - i, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + (y - i) + " ";
					break;
				}else if(i != 0 && board[y - i, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(y - i >= 0 && x - i < 8){
				if(board[y - i, x - i] == null){
					vals += (x - i) + " " + (y - i) + " ";
					}else if(board[y - i, x - i].isWhite != board[y, x].isWhite){
					vals += (x - i) + " " + (y - i) + " ";
					break;
				}else if(i != 0 && board[y - i, x - i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			//Horizontal and Vertical Checks
			while(y + i >= 0){
				if(board[y + i, x] == null){
					vals += x + " " + (y + i) + " ";
					}else if(board[y + i, x].isWhite != board[y, x].isWhite){
					vals += x + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x].isWhite == board[y, x].isWhite){
					break;
				}
				i--;
			}
			i = 0;
			
			while(y + i < 8){
				if(board[y + i, x] == null){
					vals += x + " " + (y + i) + " ";
					}else if(board[y + i, x].isWhite != board[y, x].isWhite){
					vals += x + " " + (y + i) + " ";
					break;
				}else if(i != 0 && board[y + i, x].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
			i = 0;
			
			while(x + i >= 0){
				if(board[y, x + i] == null){
					vals += (x + i) + " " + y + " ";
					}else if(board[y, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + y + " ";
					break;
				}else if(i != 0 && board[y, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i--;
			}
			i = 0;
			
			while(x + i < 8){
				if(board[y, x + i] == null){
					vals += (x + i) + " " + y + " ";
					}else if(board[y, x + i].isWhite != board[y, x].isWhite){
					vals += (x + i) + " " + y + " ";
					break;
				}else if(i != 0 && board[y, x + i].isWhite == board[y, x].isWhite){
					break;
				}
				i++;
			}
		}
		
		if(board[y, x].name == "king"){
			GD.Print(board[y, x].toString());
			if(x - 1 >= 0 && y - 1 >= 0){
				if(board[y - 1, x - 1] == null || board[y - 1, x - 1].isWhite != board[y, x].isWhite){
					vals += (x - 1) + " " + (y - 1) + " ";
				}
				if(board[y - 1, x] == null || board[y - 1, x].isWhite != board[y, x].isWhite){
					vals += x + " " + (y - 1) + " ";
				}
			}
			if(x + 1 < 8 && y - 1 >= 0){
				if(board[y - 1, x + 1] == null || board[y - 1, x + 1].isWhite != board[y, x].isWhite){
					vals += (x + 1) + " " + (y - 1) + " ";
				}
				if(board[y, x + 1] == null || board[y, x + 1].isWhite != board[y, x].isWhite){
					vals += (x + 1) + " " + y + " ";
				}
			}
			if(x + 1 >= 0 && y + 1 >= 0){
				if(board[y + 1, x + 1] == null || board[y + 1, x + 1].isWhite != board[y, x].isWhite){
					vals += (x + 1) + " " + (y + 1) + " ";
				}
				if(board[y + 1, x] == null || board[y + 1, x].isWhite != board[y, x].isWhite){
					vals += x + " " + (y + 1) + " ";
				}
			}
			if(x - 1 >= 0 && y + 1 >= 0){
				if(board[y + 1, x - 1] == null || board[y + 1, x - 1].isWhite != board[y, x].isWhite){
					vals += (x - 1) + " " + (y + 1) + " ";
				}
				if(board[y, x - 1] == null || board[y, x - 1].isWhite != board[y, x].isWhite){
					vals += (x - 1) + " " + y + " ";
				}
			}
		}
		
		
		return vals;
	}
	
	public bool isCheck(int x1, int y1, int x2, int y2){
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				if(board[j, i].isWhite != board[y1, x1].isWhite){
					
				}
			}
		}
		
		return false;
	}
	
	
	//Loops through the board to generate a Fen-style string of the data
	public string CreateFenString(){
		string fenString = "";
		int emptySpace = 0;
		
		for(int y = 0; y < 8; y++){
			emptySpace = 0;
			
			for(int x = 0; x < 8; x++){
				if(board[y, x] != null){
					if(emptySpace > 0){
						fenString += emptySpace.ToString();
						emptySpace = 0;
					}
					if(board[y, x].name == "knight"){
						if(board[y, x].isWhite == true){
							fenString += "N";
						}else{
							fenString += "n";
						}
					}else{
						if(board[y, x].isWhite == true){
							fenString += Char.ToUpper(board[y, x].name[0]);
						}else{
							fenString += board[y, x].name[0];
						}
					}
				}else{
					emptySpace += 1;
				}
				if(emptySpace > 0 && x == 7){
					fenString += emptySpace.ToString();
					emptySpace = 0;
				}
			}
			if(y < 7){
				fenString += "/";
			}
		}
		
		fenString += " ";
		if(isWhitesTurn){
			fenString += "w ";
		}else{
			fenString += "b ";
		}
		
		if(canCastle[0]){
			fenString += "K";
		}
		if(canCastle[1]){
			fenString += "Q";
		}
		if(canCastle[2]){
			fenString += "k";
		}
		if(canCastle[3]){
			fenString += "q";
		}
		
		//If there is EnPassant on the board
		//fenString += thatSpace;
		fenString += " - ";
		
		fenString += turn + " " + halfTurn;
		
		return fenString;
	}
}
