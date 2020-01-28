using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Convert;

public class GameBoard : MonoBehaviour
{
    private readonly int NUM_ROWS = 61;
    private readonly int NUM_COLUMNS = 115;
    private readonly float TILE_SIZE = 1;
    internal Trump trump;
    internal Pelosi pelosi;
    private readonly float TRUMP_START_X = 97, TRUMP_START_Y = -25, TRUMP_START_Z = -2; 
    private readonly float PELOSI_START_X = 4, PELOSI_START_Y = -24, PELOSI_START_Z = -2;
    private readonly float LADDER_X_LOCATION = 38, LADDER_Y_LOCATION = -48;
    private Tileable[,] gameBoard;
    private EmptyTile[,] emptyBoardTiles;
    internal List<Tileable> highlightedTiles = new List<Tileable>();
    private Tileable playerToMove;
    private int manhattanDistanceMax = 10;


    // Start is called before the first frame update
    void Start()
    {
        CreateEmptyBoard();
        AddStartingCharacters();
    }

    // Readies tiles that could be moved to
    internal void AwaitMoveSelection(Tileable player)
    {
        foreach(Tileable potentialChoice in highlightedTiles)
        {
            potentialChoice.IsAwaitingSelection(true);
        }
        playerToMove = player;
    }

    internal void MovePlayerToTile(Tileable destination)
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

    // Adds trump and pelosi to the board
    private void AddStartingCharacters()
    {
        // Add trump
        GameObject trumpImage = Instantiate(Resources.Load("Prefabs/trump") as GameObject);
        trump = trumpImage.AddComponent<Trump>();
        trump.Initialize(this, TRUMP_START_X, TRUMP_START_Y, TRUMP_START_Z);
            

        // Add pelosi
        GameObject pelosiImage = Instantiate(Resources.Load("Prefabs/pelosi") as GameObject);
        pelosi = pelosiImage.AddComponent<Pelosi>();
        pelosi.Initialize(this, PELOSI_START_X, PELOSI_START_Y, PELOSI_START_Z);
    }

    // Checks for Game Ending conditions
    public void CheckForEndGame()
    {
        // Check if trump has reached the ladder
        if(trump.GetXLocation() == LADDER_X_LOCATION && trump.GetYLocation() == LADDER_Y_LOCATION)
        {
            Debug.Log("You Won! Trump escaped Pelosi.\n");
        }

        // Check if pelosi captured trump
        if (trump.GetXLocation() == pelosi.GetXLocation() && trump.GetYLocation() == pelosi.GetYLocation())
        {
            Debug.Log("You Lost! Pelosi gotcha.\n");
        }
    }

    // Must be called before the tile has it's location updated
    public void UpdateGameBoard(Tileable tile, int newColumn, int newRow)
    {
        // Make tile's old board location empty
        int previousTileX = RoundXCoordToInt(tile.GetXLocation());
        int previousTileY = RoundYCoordToPosInt(tile.GetYLocation());
        gameBoard[previousTileX, previousTileY] = emptyBoardTiles[previousTileX, previousTileY];

        // Give tile new location on gameboard
        gameBoard[newColumn, newRow] = tile;
    }

    // Highlights potential moves and readys selectable tiles to be clicked
    public void HighlightAvailableTiles(Tileable origin)
    {
        int distanceToDestination;

        foreach(Tileable destinationTile in gameBoard)
        {
            distanceToDestination = calculateManhattanDist(destinationTile, origin);

            if (distanceToDestination <= manhattanDistanceMax && destinationTile.IsOccupiable())
            {
                destinationTile.Highlight();
                highlightedTiles.Add(destinationTile);
            }
            origin.RemoveHighLight();
        }

        AwaitMoveSelection(origin);
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
        trump.IsAwaitingSelection(false);
    }

    // Returns the manhattan distance between two tiles
    private int calculateManhattanDist(Tileable destination, Tileable origin)
    {
        int xDifference = Math.Abs((int)(destination.GetXLocation() - origin.GetXLocation()));
        int yDifference = Math.Abs((int)(destination.GetYLocation() - origin.GetYLocation()));

        return xDifference + yDifference;
    }

}
