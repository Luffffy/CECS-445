using UnityEngine;
using static GridConverter;

// This class outlines all computer controlled units, I'm not sure if computer controlled
// assets are different enough from player controlled units to justify their own class but here we are. 
public class ComputerControlledUnit : Unit
{
    // Handles Mouse Clicks
    public override void OnMouseDown()
    {
        currentState.TakeAction();
        // TODO: This is an example todo in Visual Studios, can be seen in task list. Tile collisions needs to be handled.
        Debug.Log("Computer and user fight to the death, you lose.");
    }

    // Moves unit right 5 spaces for now
    public void Move()
    {
        this.SetLocation(xCoordinate + 5, yCoordinate, zCoordinate);
        currentState = states.unitIdle;
    }

    // Highlights the units tile with a preset color
    public override void Highlight()
    {
        throw new System.NotImplementedException();
    }

    // Removes any highlights applied to the units tile
    public override void RemoveHighLight()
    {
        graphicRenderer.material.color = unhighlightedColor;
        currentState = states.unitIdle;
    }
}
