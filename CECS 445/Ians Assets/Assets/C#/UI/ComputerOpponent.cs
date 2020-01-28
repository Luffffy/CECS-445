using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridConverter;

public class ComputerOponent : MonoBehaviour, Tileable, Player
{
    private float xCoordinate, yCoordinate, zCoordinate;
    int column, row;
    private Map gameBoard;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
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

    // Simulates a fight in the console for now
    public void OnMouseDown()
    {
        // TODO: This is an example todo in Visual Studios, can be seen in task list. Trump/pelosi collision needs to be handle differently.
        Debug.Log("Computer and user fight to the death, you lose.");
    }

    // Moves unit right 5 spaces for now
    public void Move()
    {
        this.SetLocation(xCoordinate + 5, yCoordinate, zCoordinate);
    }

    public void Highlight()
    {
        throw new System.NotImplementedException();
    }

    // Creates an opponent, sets their location, adds a box collider
    public void Initialize(Map gameBoard, float xLocation, float yLocation, float zLocation)
    {
        this.gameBoard = gameBoard;
        SetLocation(xLocation, yLocation, zLocation);
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

    public void IsAwaitingSelection(bool awaitingStatus)
    {
        throw new System.NotImplementedException();
    }
}
