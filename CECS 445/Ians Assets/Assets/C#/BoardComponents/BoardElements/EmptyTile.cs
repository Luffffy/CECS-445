using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class EmptyTile : MonoBehaviour, Tileable
{
    private float xCoordinate, yCoordinate, zCoordinate;
    Renderer rend;
    bool isAPotentialMoveSelection = false;
    bool isAwaitingSelection = false;
    USAGameBoard gameBoard;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckForCursorHover();
    }

    // Accepts a user move if awaitingSelection, initiates the computer's turn
    public void OnMouseDown()
    {
        if (this.isAwaitingSelection)
        {
            gameBoard.MovePlayerToTile(this);
            gameBoard.computerPlayer.Move();
        }
    }

    // Moves tile to a specific location on the board
    public void SetLocation(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;

        this.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
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

    // Changes the tile color to green
    public void Highlight()
    {
        this.isAPotentialMoveSelection = true;
        rend.material.color = Color.green;
    }

    // Changes the tile the cursor is over to blue
    public void HighlightMouseLocation()
    {
        rend.material.color = Color.blue;
    }

    // Removes highlight
    public void RemoveHighLight()
    {
        rend.material.color = Color.white;
        this.isAPotentialMoveSelection = false;
    }

    // Highlights a square with a custom color
    private void CustomHighLight(Color color)
    {
        rend.material.color = color;
    }

    // Checks if the cursor is hovering on this tile
    private void CheckForCursorHover()
    {
        Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordinate, cursorLocation.y - yCoordinate);

        // If cursor is inside the tile: highlight the tile
        if (cursorIsOnTile)
        {
            HighlightMouseLocation();
        }
        // Keep the tile highlighted green if its a potential move
        if(!cursorIsOnTile && this.isAPotentialMoveSelection)
        {
            CustomHighLight(Color.green);
        }
        // Remove all highlights
        if(!cursorIsOnTile && !this.isAPotentialMoveSelection)
        {
            RemoveHighLight();
        }
    }

    // Creates an empty tile, sets its location, adds a box collider
    public void Initialize(USAGameBoard gameBoard, float xLocation, float yLocation, float zLocation)
    {
        SetLocation(xLocation, yLocation, zLocation);
        this.gameBoard = gameBoard;
        this.gameObject.AddComponent(typeof(BoxCollider));
    }

    // Returns a bool indicating if the a player can move to this tile
    public bool IsOccupiable()
    {
        return true;
    }

    // Returns a bool indicating if the cursor is hover on tile
    private bool CursorIsOnTile(float mouseXLocation, float mouseYLocation)
    {
        if ((-0.5f < mouseXLocation && mouseXLocation < 0.5) && (-0.5 < mouseYLocation && mouseYLocation < 0.5))
        {
            return true;
        }
        return false;
    }

    // Sets bool isAwaitingSelection which alters OnMouseDown() actions
    public void IsAwaitingSelection(bool awaitingStatus)
    {
        this.isAwaitingSelection = awaitingStatus;
    }
}

