using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GridConverter;

// This class acts as a controller for all the units on the gameboard, this is probably a terrible idea and may
// eventually mean this class balloons into some sort of monstrocity.
public class GameBoard : MonoBehaviour, Observer
{
    private readonly int NUM_ROWS = 61, NUM_COLUMNS = 115;
    private readonly float TILE_SIZE = 1;
    private readonly float VICTORY_X_LOCATION = 38, VICTORY_Y_LOCATION = -48;
    private readonly string VICTORY_MESSAGE = "You Won! User escaped the computer.\n";
    private readonly string DEFEAT_MESSAGE = "You Lost! AI gotcha.\n";

    internal UserControlledUnit userControlledUnit;
    internal ComputerControlledUnit computerControlledUnit;

    private Tileable[,] gameBoard;
    private EmptyTile[,] emptyBoardTiles;
    private List<Tileable> highlightedTiles = new List<Tileable>();
    private Unit unitToMove;

    // Start is called before the first frame update
    void Start()
    {
        CreateEmptyBoard();
        AddStartingCharacters();
    }

    // Moves a unit to a tile, resets any highlights, checks for end game
    public void MoveUnitToTile(Tileable destination)
    {
        unitToMove.SetLocation(destination.GetXLocation(), destination.GetYLocation(), unitToMove.GetZLocation());
        ResetHighlightedTiles();
        CheckForEndGame();
    }

    // Creates a board with only transparent tiles. Adds tiles starting at coordinate (0,0)
    private void CreateEmptyBoard()
    {
        gameBoard = new Tileable[NUM_COLUMNS, NUM_ROWS];
        emptyBoardTiles = new EmptyTile[NUM_COLUMNS, NUM_ROWS];  // Save an empty tile for each position on the board for easy reuse        

        // Fill all grid spaces with transparent (Empty) tiles
        for (int row = 0; row < NUM_ROWS; row++)
        {
            for (int column = 0; column < NUM_COLUMNS; column++)
            {
                float xCoordinate = column * TILE_SIZE;
                float yCoordinate = row * -TILE_SIZE;
                float zCoordinate = -1.0f;

                // Create the tile objects
                GameObject gridSquare = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                EmptyTile emptyTile = gridSquare.AddComponent<EmptyTile>();
                emptyTile.Initialize(this, xCoordinate, yCoordinate, zCoordinate);

                // Save the objects to the board
                gameBoard[column, row] = emptyTile;
                emptyBoardTiles[column, row] = emptyTile;
            }
        }
    }

    // Creates and adds starting units to the board
    private void AddStartingCharacters()
    {
        float USER_START_X = 97, USER_START_Y = -25, USER_START_Z = -2;
        float COMPUTER_START_X = 4, COMPUTER_START_Y = -24, COMPUTER_START_Z = -2;
   
        // Add user controlled unit
        GameObject userControlledUnitImage = Instantiate(Resources.Load("Prefabs/trump") as GameObject);
        userControlledUnit = userControlledUnitImage.AddComponent<UserControlledUnit>();
        userControlledUnit.Initialize(this, USER_START_X, USER_START_Y, USER_START_Z);     

        // Add computer controlled unit
        GameObject computerControlledUnitImage = Instantiate(Resources.Load("Prefabs/pelosi") as GameObject);
        computerControlledUnit = computerControlledUnitImage.AddComponent<ComputerControlledUnit>();
        computerControlledUnit.Initialize(this, COMPUTER_START_X, COMPUTER_START_Y, COMPUTER_START_Z);
    }

    // Checks for Game Ending conditions
    public void CheckForEndGame()
    {
        // Check if user controlled unit has reached the objective
        if(userControlledUnit.GetXLocation() == VICTORY_X_LOCATION && userControlledUnit.GetYLocation() == VICTORY_Y_LOCATION)
        {
            Debug.Log(VICTORY_MESSAGE);
        }

        // Check if the computer has captured the unit
        if (userControlledUnit.GetXLocation() == computerControlledUnit.GetXLocation() && userControlledUnit.GetYLocation() == computerControlledUnit.GetYLocation())
        {
            Debug.Log(DEFEAT_MESSAGE);
        }
    }

    // Must be called before the tile has it's location updated
    private void RecordTileMovement(Tileable itemToMove)
    {
        // Make the tile the item is moving from empty
        int previousItemX = RoundXCoordToInt(itemToMove.GetXLocation());
        int previousItemY = RoundYCoordToPosInt(itemToMove.GetYLocation());
        int newColumn = RoundXCoordToInt(itemToMove.GetFutureXLocation());
        int newRow = RoundYCoordToPosInt(itemToMove.GetFutureYLocation());

        gameBoard[previousItemX, previousItemY] = emptyBoardTiles[previousItemX, previousItemY];

        // Give tile new location on gameboard
        gameBoard[newColumn, newRow] = itemToMove;
    }

    // Highlights potential moves and readys selectable tiles to be clicked
    public void HighlightPotentialMoves(Unit unit)
    {
        int distanceToDestination;
        this.unitToMove = unit;

        foreach(Tileable tile in gameBoard)
        {
            distanceToDestination = CalculateManhattanDist(tile, unit);

            // Highlight tile if elligible
            if (distanceToDestination <= unit.GetMaxMoveDistance() && tile.IsOccupiable())
            {
                tile.Highlight();
                highlightedTiles.Add(tile);
            }
            unit.RemoveHighLight();  // Remove highlight on currently selected tile
        }
    }

    // Sets all highlighted tiles to their default
    public void ResetHighlightedTiles()
    {
        foreach(Tileable tile in highlightedTiles)
        {
            tile.RemoveHighLight();
        }
        unitToMove.SetState(unitToMove.states.unitIdle);
        highlightedTiles.Clear();
    }

    // Returns the manhattan distance between two tiles
    private int CalculateManhattanDist(Tileable destination, Tileable origin)
    {
        int xDifference = Math.Abs((int)(destination.GetXLocation() - origin.GetXLocation()));
        int yDifference = Math.Abs((int)(destination.GetYLocation() - origin.GetYLocation()));

        return xDifference + yDifference;
    }

    // Observer pattern - receive updates from subjects
    public void ReceiveUpdate(UnityEngine.Object sender, Message message)
    {
        switch (message)
        {
            case Message.PositionChanged:
                RecordTileMovement(sender as Tileable);
                ResetHighlightedTiles();  // Resets highlighted tiles for now, will need to change if more than one move per turn is allowed 
                CheckForEndGame();
                break;

            case Message.TileClicked:
                MoveUnitToTile(sender as Tileable);
                ResetHighlightedTiles();  
                CheckForEndGame();
                break;
        }
    }

    public void SetUnitToMove(Unit unit)
    {
        this.unitToMove = unit;
    }
}
