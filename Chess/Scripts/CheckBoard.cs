using Godot;
using System;

public partial class CheckBoard : Sprite2D
{
	private PackedScene Piece;
	private PackedScene BoardButton;
	public string fenPosition{get;set;}
	public Piece[] board;
	public bool isWhitesTurn{get;set;}
	public bool[] canCastle = {false, false, false, false};
	public int turn;
	public int halfTurn;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fenPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
		Piece = ResourceLoader.Load<PackedScene>("res://Scenes/piece.tscn");
		BoardButton = ResourceLoader.Load<PackedScene>("res://Scenes/BoardButton.tscn");
		
		//Sets up the board. empty spaces are nulls
	board = new Piece[64];
		for(int i = 0; i < 64; i++){
			board[i] = null;
			GenerateBoardButton(i);
			
		}
		
		//
		GD.Print(fenPosition);
		LoadBoard(fenPosition);
		GD.Print(CreateFenString() + "\n");
		

		GD.Print(CheckMoves(8));
		GD.Print(CheckMoves(48));
		
		GD.Print(CheckMoves(0));
		MovePiece(0, 36);
		GD.Print(CheckMoves(36));
		MovePiece(36, 0);
		
		GD.Print(CheckMoves(1));
		MovePiece(1, 35);
		GD.Print(CheckMoves(35));
		MovePiece(35, 2);
		
		GD.Print(CheckMoves(2));
		MovePiece(2, 32);
		GD.Print(CheckMoves(32));
		MovePiece(32, 2);
		
		GD.Print(CheckMoves(3));
		MovePiece(3, 36);
		GD.Print(CheckMoves(36));
		MovePiece(36, 3);
		
		GD.Print(CheckMoves(4));
		MovePiece(4, 36);
		GD.Print(CheckMoves(36));
		MovePiece(36, 4);
		
	
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
		
		for(i = 0; i < 64; i++){
			if(data[0] == '/'){
				i -= 1;
			}
			if(char.IsLetter(data[0])){
				board[i] = GeneratePiece(data[0], i);
			}
			else if(char.IsDigit(data[0])){
				string temp = data.Substring(0, 1);
				int value = Int32.Parse(temp);
				
				for(int z = 0; z < value; z++){
					if(board[i + z] != null){
						RemoveChild(board[i + z]);
						board[i + z] = null;
					}
				}
				i += value - 1;
			}
			data = data.Substring(1);
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
	
	private void GenerateBoardButton(int boardPos) {
		BoardButton button = (BoardButton)BoardButton.Instantiate();
		AddChild(button);
		button.InitializeBoardButton(boardPos);
	}
	
	//This method is used to create pieces
	public Piece GeneratePiece(char type, int index){
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

		myPiece.index = index;

		AddChild(myPiece);
		myPiece.InitializePiece();

		return myPiece;
	}
	
	//moves a piece from one position to another
	public void MovePiece(int index1, int index2){
		if(board[index2] == null){
			board[index2] = board[index1];
			board[index2].index = index2;
			board[index2].InitializePiece();
			board[index1] = null;
		}else if(board[index1].isWhite != board[index2].isWhite){
			//Remove the piece at x2 y2
			RemoveChild(board[index2]);
			board[index2] = board[index1];
			board[index2].index = index2;
			board[index2].InitializePiece();
			board[index1] = null;
		}else{
			GD.Print("Invalid Move!");
		}
	}
	
	
	//Checks the possible moves of a piece on a given coordinate
	public string CheckMoves(int index){
		string vals = "";

		GD.Print(board[index].toString());

		if(board[index].name == "pawn"){
			vals = GetPawnMoves(index, vals);
		}
		else if(board[index].name == "rook"){
			vals = GetRookMoves(index, vals);
		}
		else if(board[index].name == "knight"){
			vals = GetKnightMoves(index, vals);
		}
		else if(board[index].name == "bishop"){
			vals = GetBishopMoves(index, vals);
		}
		else if(board[index].name == "queen"){
			vals = GetQueenMoves(index, vals);
		}
		else if(board[index].name == "king"){
			vals = GetKingMoves(index, vals);
		}
		
		return vals;
	}

	private string GetPawnMoves(int index, string vals) {
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
		return vals;
	}

	private string GetRookMoves(int index, string vals) {
		for(int i = index; i >= 0; i -= 8){
			if(i != index && (i % 8 == 0 || i % 8 == 7)){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		for(int i = index; i < 64; i += 8){
			if(i % 8 == 0 || i % 8 == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		for(int i = index; i >= 0; i--){
			if(i % 8 == 0 || i % 8 == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		for(int i = index; i < 64; i++){
			if(i % 8 == 0 || i % 8 == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		return vals;
	}

	private string GetKnightMoves(int index, string vals) {
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
		if(index + 170< 64 && (board[index + 10] == null || board[index + 10].isWhite != board[index].isWhite)){
			vals += (index + 10) + " ";
		}
		if(index + 6 < 64 && (board[index + 6] == null || board[index + 6].isWhite != board[index].isWhite)){
			vals += (index + 6) + " ";
		}
		return vals;
	}

	private string GetBishopMoves(int index, string vals) {
		for(int i = index; i < 64; i += 9){
			if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)index / 8)) == 0 || ((int)Math.Floor((double)index / 8)) == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		for(int i = index; i < 64; i += 7){
			if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)index / 8)) == 0 || ((int)Math.Floor((double)index / 8)) == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		for(int i = index; i >= 0; i -= 9){
			if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)index / 8)) == 0 || ((int)Math.Floor((double)index / 8)) == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		for(int i = index; i >= 0; i -= 7){
			if(i % 8 == 0 || i % 8 == 7 || ((int)Math.Floor((double)index / 8)) == 0 || ((int)Math.Floor((double)index / 8)) == 7){
				vals += i + " ";
				break;
			}
			if(board[i] == null){
				vals += i + " ";
			}else if(board[i].isWhite != board[index].isWhite){
				vals += i + " ";
				break;
			}else if(i != index && board[i].isWhite == board[index].isWhite){
				break;
			}
		}
		return vals;
	}	

	private string GetQueenMoves(int index, string vals) {
		
		GetRookMoves(index, vals);
		GetBishopMoves(index, vals);

		return vals;
	}	

	private string GetKingMoves(int index, string vals) {
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
			vals += (index + 17) + " ";
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
			return vals;
	}

	public bool isCheck(int index1, int index2){
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				if(board[index2].isWhite != board[index1].isWhite){
					
				}
			}
		}
		
		return false;
	}
	
	
	//Loops through the board to generate a Fen-style string of the data
	public string CreateFenString(){
		string fenString = "";
		int emptySpace = 0;
		
		for(int i = 0; i < 64; i++){
			if(i % 8 == 0 && i != 0){
				if(emptySpace > 0){
					fenString += emptySpace.ToString();
					emptySpace = 0;
				}
				fenString += "/";
			}
			else if(board[i] != null){
				if(emptySpace > 0){
					fenString += emptySpace.ToString();
					emptySpace = 0;
				}
				if(board[i].name == "knight"){
					if(board[i].isWhite == true){
						fenString += "N";
					}else{
						fenString += "n";
					}
				}else{
					if(board[i].isWhite == true){
						fenString += Char.ToUpper(board[i].name[0]);
					}else{
						fenString += board[i].name[0];
					}
				}
			}
			else{
				emptySpace += 1;
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
