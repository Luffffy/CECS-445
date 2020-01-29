using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridConverter;


public class UserPlayer : MonoBehaviour, Tileable, Player, Subject
{
    private float xCoordinate, yCoordinate, zCoordinate;
    private float futureXCoord, futureYCoord, futureZCoord;
    int column, row;
    private Map gameBoard;
    Renderer rend;
    bool isAlreadyClicked = false;
    List<Observer> observers = new List<Observer>();

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
        futureXCoord = xCoordinate;
        futureYCoord = yCoordinate;
        futureZCoord = zCoordinate;

        NotifyObservers();

        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.zCoordinate = zCoordinate;
        this.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Highlight()
    {
        throw new System.NotImplementedException();
    }

    // Creates an opponent, sets their location, adds a box collider
    public void Initialize(Map gameBoard, float xCoordinate, float yCoordinate, float zCoordinate)
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

    public void AttachObserver(Observer o)
    {
        observers.Add(o);
    }

    public void DettachObserver(Observer o)
    {
        observers.Remove(o);
    }

    public void NotifyObservers()
    {
        foreach(Observer observer in observers)
        {
            observer.ReceiveUpdate(this);
        }
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
}
