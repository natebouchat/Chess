using Godot;
using System;

public partial class CheckBoard : Sprite2D
{
	private PackedScene Piece;
	public string fenPosition{get;set;}
	Piece[,] board;
	
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
		
		LoadBoard(fenPosition);
		
		CreateFenString();	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void LoadBoard(string fenData){
		string data = fenData.Substring(0, fenData.IndexOf(' '));
		//GD.Print("-" + data + "-");
		
		int i = 0;
		int j = 0;
		for(i = 0; i < 8; i++){
			j = 0;
			while(data.Length > 0 && data[0] != '/'){
				if(char.IsLetter(data[0]) && j < 8){
					//GD.Print(data[0] + ", " + i + ", " + j);
					board[i, j] = GeneratePiece(data[0], i, j);
				}else if(char.IsDigit(data[0])){
					//GD.Print(data[0] + ", " + i + ", " + j);
					string temp = data.Substring(0, 1);
					//GD.Print("-" + temp + "-");
					int value = Int32.Parse(temp);
					//GD.Print(value);
					
					j += value - 1;
				}
					data = data.Remove(0, 1);
				j++;
			}
			if(data.Length > 0){
				data = data.Remove(0, 1);
				//GD.Print(data);
			}
			//GD.Print("Exited the loop for the " + i + " Time!");
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
		
		return myPiece;
	}
	
	
	public string CreateFenString(){
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				if(board[i, j] != null){
					GD.Print(i + ", " + j + ": " + board[i, j].name);
				}else{
					GD.Print(i + ", " + j + ": EMPTY");
				}
			}
		}
		
		return "FIX ME";
	}
}
