using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

// This class outlines an empty tile which responds to mouse actions.
public class EmptyTile : MonoBehaviour, Tileable, Observable
{
    private float xCoordinate, yCoordinate, zCoordinate;
    Renderer graphicRenderer;
    public GameBoard gameBoard;
    TileStatesFactory states;
    State currentState;
    protected List<Observer> observers = new List<Observer>();

    void Update()
    {
        CheckForCursorHover();
    }

    // Handles left clicks
    public void OnMouseDown()
    {
        currentState.TakeAction();
    }

    // Moves tile to a specific location on the board
    public void SetLocation(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;

        this.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }

    // Changes the tile color to green
    public void Highlight()
    {
        graphicRenderer.material.color = Color.green;
        currentState = states.awaitingMove;
    }

    // Changes the tile the cursor is over to blue
    public void HighlightCursorLocation()
    {
        graphicRenderer.material.color = Color.blue;
        currentState = states.tileHighlighted;
    }

    // Removes highlight
    public void RemoveHighLight()
    {
        graphicRenderer.material.color = Color.white;
        currentState = states.tileIdle;
    }

    // Highlights a square with a custom color
    private void CustomHighLight(Color color)
    {
        graphicRenderer.material.color = color;
        currentState = states.awaitingMove;
    }

    // Checks if the cursor is hovering on this tile
    private void CheckForCursorHover()
    {
        Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // Get cursor location
        bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordinate, cursorLocation.y - yCoordinate);

        // If cursor is inside tile that can't be picked: highlight the tile
        if (cursorIsOnTile  && currentState != states.awaitingMove)
        {
            HighlightCursorLocation();
        }

        // If cursor is inside tile that can be picked: highlight the tile
        if (cursorIsOnTile && currentState == states.awaitingMove)
        {
            CustomHighLight(Color.blue);
        }

        // If cursor was on highlighted tile and has moved, recover highlight
        if (!cursorIsOnTile && currentState == states.awaitingMove)
        {
            CustomHighLight(Color.green);
        }
        // Remove all highlights
        if (!cursorIsOnTile && currentState == states.tileHighlighted)
        {
            RemoveHighLight();
        }
    }

    // Creates an empty tile, sets its location, adds a box collider
    public void Initialize(GameBoard gameBoard, float xLocation, float yLocation, float zLocation)
    {
        SetLocation(xLocation, yLocation, zLocation);
        this.gameBoard = gameBoard;
        this.gameObject.AddComponent(typeof(BoxCollider));
        graphicRenderer = GetComponent<Renderer>();
        observers.Add(gameBoard);

        states = new TileStatesFactory(this);
        currentState = states.tileIdle;
    }

    // Returns a bool indicating if the a player can move to this tile
    public bool IsOccupiable()
    {
        return true;
    }

    // Returns a bool indicating if the cursor is hovering on tile
    private bool CursorIsOnTile(float mouseXLocation, float mouseYLocation)
    {
        if ((-0.5f < mouseXLocation && mouseXLocation < 0.5) && (-0.5 < mouseYLocation && mouseYLocation < 0.5))
        {
            return true;
        }
        return false;
    }

    public float GetFutureXLocation()
    {
        throw new NotImplementedException();
    }

    public float GetFutureYLocation()
    {
        throw new NotImplementedException();
    }

    public float GetFutureZLocation()
    {
        throw new NotImplementedException();
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

    // Adds an observer to this class
    public void AttachObserver(Observer o)
    {
        observers.Add(o);
    }

    // Removes an observer from this class
    public void DettachObserver(Observer o)
    {
        observers.Remove(o);
    }

    // Alerts all observers to a positional change
    public void NotifyObservers()
    {
        foreach (Observer observer in observers)
        {
            observer.ReceiveUpdate(this, Message.TileClicked);
        }
    }
}

