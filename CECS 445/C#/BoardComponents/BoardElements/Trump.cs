using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Convert;


public class Trump : MonoBehaviour, Tileable, Player
{
    private float xCoordinate, yCoordinate, zCoordinate;
    int column, row;
    private GameBoard gameBoard;
    Renderer rend;
    bool isAlreadyClicked = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Hides/Shows trump's potential moves
    public void OnMouseDown()
    {
        // Hide trump's possible moves
        if (this.isAlreadyClicked)
        {
            isAlreadyClicked = false;
            gameBoard.ResetHighlightedTiles();
        }
        // Show trump's possible moves
        else
        {
            this.isAlreadyClicked = true;
            gameBoard.HighlightAvailableTiles(this);
        }
    }

    // Moves tile and updates the gameboard
    public void SetLocation(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        column = RoundXCoordToInt(xCoordinate);
        row = RoundYCoordToPosInt(yCoordinate);

        gameBoard.UpdateGameBoard(this, column, row);

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

    public void Initialize(GameBoard gameBoard, float xCoordinate, float yCoordinate, float zCoordinate)
    {
        this.gameBoard = gameBoard;
        SetLocation(xCoordinate, yCoordinate, zCoordinate);
        this.gameObject.AddComponent(typeof(BoxCollider));
    }

    public bool IsOccupiable()
    {
        return false;
    }

    // Removes highlight
    public void RemoveHighLight()
    {
        rend.material.color = Color.white;
    }

    public void IsAwaitingSelection(bool awaitingStatus)
    {
        this.isAlreadyClicked = awaitingStatus;
    }
}
