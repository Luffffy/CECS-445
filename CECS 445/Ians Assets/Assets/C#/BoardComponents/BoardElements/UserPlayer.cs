using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Convert;


public class UserPlayer : MonoBehaviour, Tileable, Player
{
    private float xCoordinate, yCoordinate, zCoordinate;
    int column, row;
    private USAGameBoard gameBoard;
    Renderer rend;
    bool isAlreadyClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Hides/Shows user's potential moves
    public void OnMouseDown()
    {
        // Hide user's possible moves
        if (this.isAlreadyClicked)
        {
            isAlreadyClicked = false;
            gameBoard.ResetHighlightedTiles();
        }
        // Show user's possible moves
        else
        {
            this.isAlreadyClicked = true;
            gameBoard.HighlightPotentialMoves(this);
        }
    }

    // Moves tile and updates the gameboard with the tile's new location
    public void SetLocation(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        column = RoundXCoordToInt(xCoordinate);
        row = RoundYCoordToPosInt(yCoordinate);

        gameBoard.RecordTileMovement(this, column, row);

        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.zCoordinate = zCoordinate;
        this.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public float GetXLocation()
    {
        return xCoordinate;
    }

    public float GetYLocation()
    {
        return yCoordinate;
    }

    public float GetZLocation()
    {
        return zCoordinate;
    }

    public void Highlight()
    {
        throw new System.NotImplementedException();
    }

    // Creates an opponent, sets their location, adds a box collider
    public void Initialize(USAGameBoard gameBoard, float xCoordinate, float yCoordinate, float zCoordinate)
    {
        this.gameBoard = gameBoard;
        SetLocation(xCoordinate, yCoordinate, zCoordinate);
        this.gameObject.AddComponent(typeof(BoxCollider));
    }

    // Returns a bool indicating if other tiles can be moved onto this player's location
    public bool IsOccupiable()
    {
        return false;
    }

    // Removes highlight
    public void RemoveHighLight()
    {
        rend.material.color = Color.white;
    }

    // Sets a boolean which controls which action this tile takes upon a mousedown event
    public void IsAwaitingSelection(bool awaitingStatus)
    {
        this.isAlreadyClicked = awaitingStatus;
    }
}
