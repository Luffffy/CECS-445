using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Convert;

public class USAGameBoard : MonoBehaviour, GameBoard
{
    private readonly int NUM_ROWS = 61;
    private readonly int NUM_COLUMNS = 115;
    private readonly float TILE_SIZE = 1;
    internal UserPlayer userPlayer;
    internal ComputerOponent computerPlayer;
    private readonly float USER_START_X = 97, USER_START_Y = -25, USER_START_Z = -2; 
    private readonly float COMPUTER_START_X = 4, COMPUTER_START_Y = -24, COMPUTER_START_Z = -2;
    private readonly float VICTORY_X_LOCATION = 38, VICTORY_Y_LOCATION = -48;
    private Tileable[,] gameBoard;
    private EmptyTile[,] emptyBoardTiles;
    internal List<Tileable> highlightedTiles = new List<Tileable>();
    private Tileable playerToMove;
    private readonly int MAX_PLAYER_MOVEMENT = 10;
    private readonly string VICTORY_MESSAGE = "You Won! Trump escaped Pelosi.\n";
    private readonly string DEFEAT_MESSAGE = "You Lost! Pelosi gotcha.\n";


    // Start is called before the first frame update
    void Start()
    {
        CreateEmptyBoard();
        AddStartingCharacters();
    }

    // Readies tiles that could be moved to
    public void AwaitUserSelection(Tileable player)
    {
        foreach(Tileable potentialChoice in highlightedTiles)
        {
            potentialChoice.IsAwaitingSelection(true);
        }
        playerToMove = player;
    }

    // Moves a player to a tile, resets any highlights, checks for end game
    public void MovePlayerToTile(Tileable destination)
    {
        playerToMove.SetLocation(destination.GetXLocation(), destination.GetYLocation(), playerToMove.GetZLocation());
        ResetHighlightedTiles();
        CheckForEndGame();
    }

    // Creates a board with only transparent tiles. Adds tiles starting at coordinate (0,0)
    private void CreateEmptyBoard()
    {
        gameBoard = new Tileable[NUM_COLUMNS, NUM_ROWS];
        emptyBoardTiles = new EmptyTile[NUM_COLUMNS, NUM_ROWS];        

        // Fill all grid spaces with transparent tiles
        for (int row = 0; row < NUM_ROWS; row++)
        {
            for (int column = 0; column < NUM_COLUMNS; column++)
            {
                float xCoordinate = column * TILE_SIZE;
                float yCoordinate = row * -TILE_SIZE;
                float zCoordinate = -1.0f;

                GameObject gridSquare = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                EmptyTile emptyTile = gridSquare.AddComponent<EmptyTile>();
                emptyTile.Initialize(this, xCoordinate, yCoordinate, zCoordinate);

                gameBoard[column, row] = emptyTile;
                emptyBoardTiles[column, row] = emptyTile;
            }
        }
    }

    // Creates and adds starting players to the board
    private void AddStartingCharacters()
    {
        // Add trump
        GameObject trumpImage = Instantiate(Resources.Load("Prefabs/trump") as GameObject);
        userPlayer = trumpImage.AddComponent<UserPlayer>();
        userPlayer.Initialize(this, USER_START_X, USER_START_Y, USER_START_Z);
            

        // Add pelosi
        GameObject pelosiImage = Instantiate(Resources.Load("Prefabs/pelosi") as GameObject);
        computerPlayer = pelosiImage.AddComponent<ComputerOponent>();
        computerPlayer.Initialize(this, COMPUTER_START_X, COMPUTER_START_Y, COMPUTER_START_Z);
    }

    // Checks for Game Ending conditions
    public void CheckForEndGame()
    {
        // Check if player has reached the objective
        if(userPlayer.GetXLocation() == VICTORY_X_LOCATION && userPlayer.GetYLocation() == VICTORY_Y_LOCATION)
        {
            Debug.Log(VICTORY_MESSAGE);
        }

        // Check if the computer has captured the player
        if (userPlayer.GetXLocation() == computerPlayer.GetXLocation() && userPlayer.GetYLocation() == computerPlayer.GetYLocation())
        {
            Debug.Log(DEFEAT_MESSAGE);
        }
    }

    // Must be called before the tile has it's location updated
    public void RecordTileMovement(Tileable tile, int newColumn, int newRow)
    {
        // Make tile's old board location empty
        int previousTileX = RoundXCoordToInt(tile.GetXLocation());
        int previousTileY = RoundYCoordToPosInt(tile.GetYLocation());
        gameBoard[previousTileX, previousTileY] = emptyBoardTiles[previousTileX, previousTileY];

        // Give tile new location on gameboard
        gameBoard[newColumn, newRow] = tile;
    }

    // Highlights potential moves and readys selectable tiles to be clicked
    public void HighlightPotentialMoves(Tileable currentTile)
    {
        int distanceToDestination;

        foreach(Tileable pickableTile in gameBoard)
        {
            distanceToDestination = CalculateManhattanDist(pickableTile, currentTile);

            if (distanceToDestination <= MAX_PLAYER_MOVEMENT && pickableTile.IsOccupiable())
            {
                pickableTile.Highlight();
                highlightedTiles.Add(pickableTile);
            }
            currentTile.RemoveHighLight();
        }

        AwaitUserSelection(currentTile);
    }

    // Sets all highlighted tiles to their default
    public void ResetHighlightedTiles()
    {
        foreach(Tileable tile in highlightedTiles)
        {
            tile.RemoveHighLight();
            tile.IsAwaitingSelection(false);
        }
        highlightedTiles.Clear();
        userPlayer.IsAwaitingSelection(false);
    }

    // Returns the manhattan distance between two tiles
    private int CalculateManhattanDist(Tileable destination, Tileable origin)
    {
        int xDifference = Math.Abs((int)(destination.GetXLocation() - origin.GetXLocation()));
        int yDifference = Math.Abs((int)(destination.GetYLocation() - origin.GetYLocation()));

        return xDifference + yDifference;
    }

}
