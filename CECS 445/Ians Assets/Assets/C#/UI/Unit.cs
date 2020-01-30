using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using static GridConverter;

public class Unit : MonoBehaviour, Observable, Tileable
{
    protected State currentState;
    public UnitStatesFactory states;
    protected List<Observer> observers = new List<Observer>();
    protected GameBoard gameBoard;
    protected int boardColumn, boardRow;
    protected Renderer graphicRenderer;
    protected Color unhighlightedColor = Color.white;
    protected Color highlightedColor = Color.green;
    protected float xCoordinate, yCoordinate, zCoordinate;
    protected float futureXCoord, futureYCoord, futureZCoord;
    private int maxUnitMovement = 10;

    // Start is called before the first frame update
    void Start()
    {
        graphicRenderer = GetComponent<Renderer>();
        states = new UnitStatesFactory(this);  // Generate all potential unit states
        currentState = states.unitIdle;
    }

    // Creates an opponent, sets their location, adds a box collider
    public void Initialize(GameBoard gameBoard, float xLocation, float yLocation, float zLocation)
    {
        this.gameBoard = gameBoard;
        SetLocation(xLocation, yLocation, zLocation);
        this.gameObject.AddComponent(typeof(BoxCollider));  // Is required in order to respond to mouse input
    }

    // Returns a bool indicating if other tiles can be moved onto this unit's location
    public bool IsOccupiable()
    {
        return false;
    }

    // Moves tile and updates the gameboard with the tile's new location, must notify observers before moving
    public void SetLocation(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        // So the board can know which tile is to be moved to
        futureXCoord = xCoordinate;
        futureYCoord = yCoordinate;
        futureZCoord = zCoordinate;

        // Tell the board it's new location
        NotifyObservers();

        // Move unit
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.zCoordinate = zCoordinate;
        this.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }

    // Handles mouse clicks
    public virtual void OnMouseDown(){}

    // Changes the tile color to a preset color
    public virtual void Highlight()
    {
        throw new System.NotImplementedException();
    }

    // Removes highlight
    public virtual void RemoveHighLight()
    {
        throw new System.NotImplementedException();
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
            observer.ReceiveUpdate(this, Message.PositionChanged);
        }
    }

    // Sets the current state
    public void SetState(State desiredState)
    {
        currentState = desiredState;
    }

    public float GetFutureXLocation()
    {
        return futureXCoord;
    }

    public float GetFutureYLocation()
    {
        return futureYCoord;
    }

    public float GetFutureZLocation()
    {
        return futureZCoord;
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

    public float GetMaxMoveDistance()
    {
        return maxUnitMovement;
    }

    public GameBoard GetGameBoard() { return gameBoard; }
    public UnitStatesFactory getStates() { return states; }
}
