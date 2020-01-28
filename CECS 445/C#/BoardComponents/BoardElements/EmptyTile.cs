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
    GameBoard gameBoard;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckForCursorHover();
    }

    public void OnMouseDown()
    {
        if (this.isAwaitingSelection)
        {
            gameBoard.MovePlayerToTile(this);
            gameBoard.pelosi.Move();
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

    private void CustomHighLight(Color color)
    {
        rend.material.color = color;
    }

    // Checks if the cursor is hover on this tile
    private void CheckForCursorHover()
    {
        Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordinate, cursorLocation.y - yCoordinate);

        // If cursor is inside the tile: highlight the tile
        if (cursorIsOnTile)
        {
            HighlightMouseLocation();
        }
        // Remove highlight if cursor is not in the tile
        if(!cursorIsOnTile && this.isAPotentialMoveSelection)
        {
            CustomHighLight(Color.green);
        }

        if(!cursorIsOnTile && !this.isAPotentialMoveSelection)
        {
            RemoveHighLight();
        }
}

    public void Initialize(GameBoard gameBoard, float xLocation, float yLocation, float zLocation)
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

    private bool CursorIsOnTile(float mouseXLocation, float mouseYLocation)
    {
        if ((-0.5f < mouseXLocation && mouseXLocation < 0.5) && (-0.5 < mouseYLocation && mouseYLocation < 0.5))
        {
            return true;
        }
        return false;
    }

    // Sets bool isAwaitingSelection which can alter OnMouseDown() actions
    public void IsAwaitingSelection(bool awaitingStatus)
    {
        this.isAwaitingSelection = awaitingStatus;
    }
}

