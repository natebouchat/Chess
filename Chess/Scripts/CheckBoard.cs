using Godot;
using System;

public partial class CheckBoard : Sprite2D
{
	private PackedScene Piece;
	public string fenPosition{get;set;}
	public Piece[,] board;
	
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
	
	
	//GD.Print(data[0] + ", " + i + ", " + j);
	//GD.Print("-" + data + "-");
	
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
				//GD.Print("-" + data + "-");
				j++;
			}
			if(data.Length > 0){
				data = data.Remove(0, 1);
			}
		}
	}
	
	//This method is used to change a pieces information
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
					//GD.Print(x + ", " + y + ": " + board[y, x].name);
					if(board[y, x].name == "knight"){
						if(board[y, x].isWhite == true){
							fenString += "K";
						}else{
							fenString += "k";
						}
					}else{
						if(board[y, x].isWhite == true){
							fenString += Char.ToUpper(board[y, x].name[0]);
						}else{
							fenString += board[y, x].name[0];
						}
					}
				}else{
					//GD.Print(x + ", " + y + ": EMPTY");
					emptySpace += 1;
				}
				if(emptySpace > 0 && x == 7){
					fenString += emptySpace.ToString();
					emptySpace = 0;
				}
				//GD.Print(fenString + "-" + emptySpace);
			}
			if(y < 7){
				fenString += "/";
			}
			//GD.Print(fenString + "-" + emptySpace);
		}
		
		return fenString;
	}
}
